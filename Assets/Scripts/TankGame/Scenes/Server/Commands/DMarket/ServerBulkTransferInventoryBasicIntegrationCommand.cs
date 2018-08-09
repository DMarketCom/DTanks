using System;
using System.Collections.Generic;
using System.Linq;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.IntegrationAPI.Request;
using SHLibrary.Logging;
using TankGame.DMarketIntegration;
using TankGame.Domain.PlayerData;
using TankGame.GameServer.Commands.DMarket.Domain;
using TankGame.Inventory.Domain;
using TankGame.Network.Messages;

namespace TankGame.GameServer.Commands.DMarket
{
    public class ServerBulkTransferInventoryBasicIntegrationCommand : ServerDMarketCommandBase
    {
        private const float kCheckTransactionInterval = 0.5f;

        private readonly DMarketApiResponseAdapter _apiResponseAdapter = new DMarketApiResponseAdapter();

        private readonly Dictionary<string, string> _pendingOperations = new Dictionary<string, string>();
        private readonly List<string> _successItemsIds = new List<string>();
        private readonly List<ItemInfo> _bulkTransferItemsList;
        private readonly ItemActionType _actionType;
        private readonly int _connectionId;

        public ServerBulkTransferInventoryBasicIntegrationCommand(List<ItemInfo> playerItemInfos, ItemActionType actionType, int connectionId)
        {
            _bulkTransferItemsList = playerItemInfos;
            _actionType = actionType;
            _connectionId = connectionId;
        }

        protected override void OnAppMsgReceived(AppMessageBase message)
        {

        }

        public override void Start()
        {
            base.Start();
            ScheduledUpdate(kCheckTransactionInterval, true);

            ReplaceItemsFromDMarket();
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();
            if (_pendingOperations.Count > 0)
            {
                CheckTransactionsStatuses();
            }
        }

        private void ReplaceItemsFromDMarket()
        {
            Dictionary<string, ItemInfo> itemsInfo = new Dictionary<string, ItemInfo>();
            foreach (var item in _bulkTransferItemsList)
            {
                var assetId = DMarketConverter.GetAssetId(item.WorldId);
                itemsInfo.Add(assetId, item);
            }

            switch (_actionType)
            {
                case ItemActionType.ToMarket:
                {
                    var items = new AssetToMarketModel[_bulkTransferItemsList.Count];
                    for (var i = 0; i < items.Length; i++)
                    {
                        var classId = DMarketConverter.GetClassId(_bulkTransferItemsList[i].ItemType);
                        var assetId = DMarketConverter.GetAssetId(_bulkTransferItemsList[i].WorldId);
                        items[i] = new AssetToMarketModel
                        {
                            assetId = assetId,
                            classId = classId
                        };
                    }

                    DMarketApi.AsyncToMarket(Model.GetPlayerMarketAccessToken(_connectionId), items,
                        (response, request) =>
                        {
                            foreach (var responseItem in response.Items)
                            {
                                _pendingOperations.Add(responseItem.OperationId, responseItem.AssetId);
                            }
                        },
                        OnItemOperationError);
                    break;
                }
                case ItemActionType.FromMarket:
                {
                    var itemsIdsArray = itemsInfo.Keys.ToArray();
                    DMarketApi.AsyncFromMarket(Model.GetPlayerMarketAccessToken(_connectionId), itemsIdsArray,
                        (response, request) =>
                        {
                            foreach (var item in response.Items)
                            {
                                _pendingOperations.Add(item.OperationId, item.AssetId);
                            }
                        },
                        OnItemOperationError);
                    break;
                }
            }
        }

        private void OnItemOperationError(Error error)
        {
            if (error.ErrorCode == ErrorCode.DMarketTokenExpired)
            {
                RetryWithRefreshToken();
            }
            else
            {
                SendLoadInventoryData(error.ErrorCode);
            }
        }

        private void RetryWithRefreshToken()
        {
            Action<ErrorCode> onErrorCallback = SendLoadInventoryData;
            RefreshMarketToken(_connectionId, ReplaceItemsFromDMarket, onErrorCallback);
        }

        private void SendLoadInventoryData(ErrorCode error = ErrorCode.None)
        {
            var dataLoadResponse = new DMarketDataLoadResponse(GetInventory(_connectionId), error);
            var answerMessage = new InventoryBasicIntegrationAnswerMessage(dataLoadResponse);
            SendMessageToClient(answerMessage, _connectionId);
        }

        private void CheckTransactionsStatuses()
        {
            if (_pendingOperations.Count == 0)
            {
                return;
            }

            var transactionIds = _pendingOperations.Keys;

            DMarketApi.CheckAsyncOperation(Model.GetPlayerMarketAccessToken(_connectionId), transactionIds.ToArray(),
                (response, request) =>
                {
                    foreach (var item in response.Items)
                    {
                        string itemAssetId = item.assetId;
                        string operationId = item.operationId;
                        var transactionStatus = _apiResponseAdapter.GetTransactionStatusType(item.status);

                        switch (transactionStatus)
                        {
                            case DMarketTransactionStatusType.Fail:
                                _pendingOperations.Remove(operationId);
                                DevLogger.Error(string.Format("Transaction for item: {0} was failed.", itemAssetId));
                                break;
                            case DMarketTransactionStatusType.Pending:
                                break;
                            case DMarketTransactionStatusType.Success:
                                _pendingOperations.Remove(operationId);
                                _successItemsIds.Add(itemAssetId);
                                break;
                        }
                    }

                    if (_pendingOperations.Count == 0)
                    {
                        if (_actionType == ItemActionType.FromMarket)
                        {
                            AddMovedItemsToInventory();
                        }
                        else
                        {
                            RemoveMovedItemsFromInventory();
                        }

                        SendLoadInventoryData();
                        Terminate();
                    }
                },
                error =>
                {
                    _pendingOperations.Clear();;
                    OnItemOperationError(error);
                });
        }

        private void AddMovedItemsToInventory()
        {
            var playerInfo = GetPlayer(_connectionId);
            foreach (var itemInfo in _bulkTransferItemsList)
            {
                playerInfo.Inventory.AddItem(new PlayerItemInfo(itemInfo.ItemType, itemInfo.WorldId));
            }

            Storage.Change(playerInfo);
        }

        private void RemoveMovedItemsFromInventory()
        {
            var playerInfo = GetPlayer(_connectionId);

            foreach (var itemInfo in _bulkTransferItemsList)
            {
                playerInfo.Inventory.RemoveItem(itemInfo.WorldId);
            }

            Storage.Change(playerInfo);
        }
    }
}