using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.IntegrationAPI.Request;
using DMarketSDK.IntegrationAPI.Request.BasicIntegration;
using DMarketSDK.Market;
using SHLibrary.Logging;
using TankGame.DMarketIntegration;
using TankGame.Domain.PlayerData;
using TankGame.GameServer.Commands.DMarket.Domain;
using TankGame.Inventory.Domain;
using TankGame.Network.Messages;

namespace TankGame.GameServer.Commands.DMarket
{
    public class ServerDMarketMovingAssetsCommand : ServerDMarketCommandBase
    {
        private const float kCheckTransactionInterval = 0.5f;

        private readonly List<MoveAssetTransactionInfo> _pendingTransactions = new List<MoveAssetTransactionInfo>();
        private readonly DMarketApiResponseAdapter _apiResponseAdapter = new DMarketApiResponseAdapter();

        public override void Start()
        {
            base.Start();
            ScheduledUpdate(kCheckTransactionInterval, true);
            _pendingTransactions.Clear();
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();
            if (_pendingTransactions.Count > 0)
            {
                CheckTransactionsStatuses();
            }
        }

        protected override void OnAppMsgReceived(AppMessageBase message)
        {
            switch (message.Type)
            {
                case AppMsgType.ItemChangingRequest:
                    OnItemChangingMsg(message as AppChangingItemsMessage);
                    break;
                case AppMsgType.DMarketTransactionCompleted:
                    OnDMarketTransaction(message as AppDMarketTransactionMessage);
                    break;
            }
        }

        private void OnItemChangingMsg(AppChangingItemsMessage message)
        {
            int connectionId = message.ConnectionId;

            if (message.ActionType != ItemActionType.ToMarket && message.ActionType != ItemActionType.FromMarket)
            {
                return;
            }

            if (!IsCanMakeDMarketOperation(message))
            {
                return;
            }

            var assetIds = new List<string>();
            var itemsInfo = new Dictionary<string, ItemInfo>();
            for (var i = 0; i < message.ItemsCount; i++)
            {
                var assetId = DMarketConverter.GetAssetId(message.GetWorldId(i));
                assetIds.Add(assetId);
                itemsInfo.Add(assetId, new ItemInfo(message.GetWorldId(i), message.GetItemType(i)));
            }

            //TODO think about request generic...  now code is duplicated
            switch (message.ActionType)
            {
                case ItemActionType.ToMarket:
                    var items = new AssetToMarketModel[assetIds.Count];
                    for (var i = 0; i < items.Length; i++)
                    {
                        var classId =  DMarketConverter.GetClassId(message.GetItemType(i));
                        items[i] = new AssetToMarketModel {
                           assetId = assetIds[i],
                           classId = classId
                        };
                    }
                    DMarketApi.AsyncToMarket(Model.GetPlayerMarketAccessToken(connectionId), items,
                        (response, request) => { AsyncMarketCallback(response.Items, message, itemsInfo); },
                        error => { OnItemOperationError(message, error); });
                    break;
                case ItemActionType.FromMarket:
                    DMarketApi.AsyncFromMarket(Model.GetPlayerMarketAccessToken(connectionId), assetIds.ToArray(),
                        (response, request) => { AsyncMarketCallback(response.Items, message, itemsInfo); },
                        error => { OnItemOperationError(message, error); });
                    break;
            }
        }

        private void AsyncMarketCallback(List<AsyncMarketResponse.ItemsAsset> items, AppChangingItemsMessage message, Dictionary<string, ItemInfo> itemsInfo)
        {
            var targetTransaction = new MoveAssetTransactionInfo(message);
            _pendingTransactions.Add(targetTransaction);
            foreach (var item in items)
            {
                targetTransaction.AddOperation(item.OperationId, itemsInfo[item.AssetId]);
            }
        }

        private void OnItemOperationError(AppChangingItemsMessage message, Error error)
        {
            if (error.ErrorCode == ErrorCode.DMarketTokenExpired)
            {
                RetryWithRefreshToken(message);
            }
            else
            {
                SendErrorAnswer(message, error.ErrorCode);
            }
        }

        private void RetryWithRefreshToken(AppChangingItemsMessage message)
        {
            RefreshMarketToken(message.ConnectionId, () => OnItemChangingMsg(message),
                error =>
                {
                    var answer = new AppChangingItemsAnswerMessage(new ItemsChangingResponse(error));
                    SendMessageToClient(answer, message.ConnectionId);
                });
        }

        private void SendErrorAnswer(AppChangingItemsMessage message, ErrorCode error)
        {
            var answer = new AppChangingItemsAnswerMessage(new ItemsChangingResponse(error));
            SendMessageToClient(answer, message.ConnectionId);
        }

        private void OnDMarketTransaction(AppDMarketTransactionMessage marketMessage)
        {
            var actionType = CovertMarketAction(marketMessage.TransactionData.TransactionType);
            var appMessage = new AppChangingItemsMessage(actionType)
                { ConnectionId = marketMessage.ConnectionId};
            for (var i = 0; i < marketMessage.TransactionData.AssetIds.Count; i++)
            {
                var assetId = marketMessage.TransactionData.AssetIds[i];
                var classId = marketMessage.TransactionData.ClassIds[i];
                var worldId = DMarketConverter.GetWorldId(assetId);
                var itemType = DMarketConverter.GetItemType(classId);
                appMessage.AddItem(worldId, itemType);
            }

            OnItemChangingMsg(appMessage);
        }

        private void CheckTransactionsStatuses()
        {
            if (_pendingTransactions.Count > 0)
            {
                for (int i = 0; i < _pendingTransactions.Count; i++)
                {
                    var transaction = _pendingTransactions[i];
                    int transactionConnectionId = transaction.Sender.ConnectionId;
                    string playerMarketAccessToken = Model.GetPlayerMarketAccessToken(transactionConnectionId);

                    DMarketApi.CheckAsyncOperation(playerMarketAccessToken, transaction.OperationIds,
                    (response, request) =>
                    {
                        var answerMessage = new AppChangingItemsAnswerMessage(new ItemsChangingResponse());

                        foreach (var transactionItem in response.Items)
                        {
                            var status = _apiResponseAdapter.GetTransactionStatusType(transactionItem.status);
                            switch (status)
                            {
                                case DMarketTransactionStatusType.Fail:
                                {
                                    answerMessage.Response.MarketError = (ErrorCode) transactionItem.transferError.code;
                                    transaction.RemoveOperation(transactionItem.operationId);
                                    break;
                                }
                                case DMarketTransactionStatusType.Success:
                                {
                                    if (transaction.IsPendingOperation(transactionItem.operationId))
                                    {
                                        OnSuccessMarketOperation(transactionItem, transaction);
                                        transaction.RemoveOperation(transactionItem.operationId);
                                    }
                                    break;
                                }
                            }
                        }

                        var transactionLog = string.Format("Transaction for connection: {0}, operation left: {1}", transaction.Sender.ConnectionId,
                            transaction.OperationsCount);
                        DevLogger.Log(transactionLog, DTanksLogChannel.GameServer);

                        if (transaction.OperationsCount == 0)
                        {
                            SendMessageToClient(answerMessage, transactionConnectionId);
                            _pendingTransactions.Remove(transaction);
                        }
                        Storage.Change(GetPlayer(transactionConnectionId));
                    },
                    error =>
                    {
                        foreach (var transactionInfo in _pendingTransactions)
                        {
                            OnItemOperationError(transactionInfo.Sender, error);
                        }
                        _pendingTransactions.Clear();
                    });
                }
            }
        }

        private void OnSuccessMarketOperation(AsyncOperationRequest.Response.ItemsAsset asset, MoveAssetTransactionInfo transaction)
        {
            DMarketTransactionOperationType operationType = _apiResponseAdapter.GetTransactionOperationType(asset.operation);

            var inventory = GetInventory(transaction.Sender.ConnectionId);
            var worldId = transaction.GetWorldId(asset.operationId);
            var item = inventory.GetItem(worldId);
            if (item == null)
            {
                item = new PlayerItemInfo(transaction.GetItemType(asset.operationId), worldId);
                inventory.AddItem(item);
            }
            item.IsInMarket = operationType == DMarketTransactionOperationType.ToMarket;
        }

        private ItemActionType CovertMarketAction(MarketMoveItemType transactionType)
        {
            switch (transactionType)
            {
                case MarketMoveItemType.FromMarket:
                    return ItemActionType.FromMarket;
                case MarketMoveItemType.ToMarket:
                    return ItemActionType.ToMarket;
                default:
                    throw new NotImplementedException("Cannot convert " + transactionType);
            }
        }

        private bool IsCanMakeDMarketOperation(AppChangingItemsMessage message)
        {
            if (string.IsNullOrEmpty(Model.GetPlayerMarketAccessToken(message.ConnectionId)))
            {
                var response = new ItemsChangingResponse(ErrorCode.EmptyDMarketAccessToken);
                var answer = new AppChangingItemsAnswerMessage(response) { Error = NetworkMessageErrorType.DMarketError };
                SendMessageToClient(answer, message.ConnectionId);
                return false;
            }

            if (message.ActionType == ItemActionType.ToMarket)
            {
                for (var i = 0; i < message.ItemsCount; i++)
                {
                    var worldId = message.GetWorldId(i);
                    var item = GetInventory(message.ConnectionId).GetItem(worldId);
                    if (item == null)
                    {
                        var error = new Error
                        {
                            ErrorCode = ErrorCode.AssetNotFound,
                            ErrorMessage = string.Format("Cannot find item with id {0} for player {1}. Need reload game",
                                worldId, Model.GetUserNameByConnectionId(message.ConnectionId))
                        };
                        OnItemOperationError(message, error);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}