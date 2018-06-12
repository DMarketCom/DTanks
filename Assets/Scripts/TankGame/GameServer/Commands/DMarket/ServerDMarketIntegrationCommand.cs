using System;
using DMarketSDK.IntegrationAPI;
using Networking.Msg;
using SHLibrary.Logging;
using Shop.Domain;
using DMarketIntegration;
using PlayerData;
using SHLibrary.Utils;

namespace GameServer.Commands
{
    public class ServerDMarketIntegrationCommand : ServerAppCommandBase
    {
        private ServerApi DMarketApi { get { return Controller.DMarkteServerApi; } }

        private readonly DMarketInfoConverter _dMarketConverter = new DMarketInfoConverter();
        
        protected override void OnAppMsgReceived(AppMessageBase message)
        {
            switch (message.Type)
            {
                case AppMsgType.GetGameTokenRequest:
                    OnGetGameTokenMsg(message as AppGetGameTokenMessage);
                    break;
                case AppMsgType.UpdateMarketToken:
                    OnAddMarketTokenMsg(message as AppUpdateMarketTokenMessage);
                    break;
                case AppMsgType.ItemChangingRequest:
                    OnItemChangingMsg(message as AppChangingItemsMessage);
                    break;
                case AppMsgType.LoadDMarketDataRequest:
                    OnLoadDMarketDataMsg(message as AppLoadDMarketDataMessage);
                    break;
            }
        }

        private void OnGetGameTokenMsg(AppGetGameTokenMessage message)
        {
            var answer = new AppGetGameTokenAnswerMessage();
            DMarketApi.GetBasicAccessToken((message as AppGetGameTokenMessage).UserId,
                    (responce, parametrs) =>
                    {
                        answer.Responce = new DMarketGameTokenResponce(responce.token,
                            responce.refreshToken);
                        SendAnswer(answer, message);
                    },
                    error =>
                    {
                        DevLogger.Warning(string.Format("Get market token error: code: {0}, message:" +
                            " {1}", error.ErrorCode, error.ErrorMessage));
                        answer.Error = NetMsgErrorType.DMarketError;
                        SendAnswer(answer, message);
                    });
        }

        private void OnAddMarketTokenMsg(AppUpdateMarketTokenMessage message)
        {
            Model.MarketToken = message.MarketToken;
            Model.MakretRefreshToken = message.RefreshMarketToken;
            Model.SetChanges();
        }

        private void OnItemChangingMsg(AppChangingItemsMessage message)
        {
            switch (message.ActionType)
            {
                case ItemActionType.FromMarket:
                    OnItemFromMarketMsg(message);
                    break;
                case ItemActionType.ToMarket:
                    OnItemToMarketMsg(message);
                    break;
            }
        }

        private void OnItemFromMarketMsg(AppChangingItemsMessage message)
        {
            if (!IsCanMakeDMarketOperation(message))
            {
                return;
            }
            var assetId = _dMarketConverter.GetAssetId(message.WorldId);
            DMarketApi.FromMarket(Model.MarketToken, assetId,
                (responce, request) =>
                {
                    var inventory = GetInventory(message.ConnectionId);
                    var item = inventory.GetItem(message.WorldId);
                    if (item == null)
                    {
                        item = new PlayerItemInfo(message.ItemType, message.WorldId);
                        inventory.AddItem(item);
                    }
                    item.IsInMarket = false;
                    OnAfterSuccesDMarketOperation(message);

                },
                error =>
                {
                    OnItemOperationError(message, error);
                });
        }

        private void OnItemToMarketMsg(AppChangingItemsMessage message)
        {
            if (!IsCanMakeDMarketOperation(message))
            {
                return;
            }
            var item = GetInventory(message.ConnectionId).GetItem(message.WorldId);
            if (item == null)
            {
                var error = new Error();
                error.ErrorCode = ErrorCode.AssetNotFound;
                error.ErrorMessage = string.Format("Cannot find item with id {0} for "
                    + "player {1}. Need reload game",
                    message.WorldId, Model.ConIdToUserName[message.ConnectionId]);
                OnItemOperationError(message, error);
                return;
            }

            var assetId = _dMarketConverter.GetAssetId(item.WorldId);
            var classId = _dMarketConverter.GetClassId(item.ItemType);
            DMarketApi.ToMarket(Model.MarketToken, assetId, classId,
                (responce, request) =>
                {
                    item.IsInMarket = true;
                    OnAfterSuccesDMarketOperation(message);
                },
                error =>
                {
                    OnItemOperationError(message, error);
                });
        }
        
        private void OnAfterSuccesDMarketOperation(AppChangingItemsMessage message)
        {
            var playerInfo = GetPlayer(message.ConnectionId);
            Storage.Change(playerInfo);

            var answer = new AppChangingItemsAnswerMessage();
            answer.Response = new ItemsChangingResponse(true);
            SendAnswer(answer, message);
            SendDmarketDataUpdateAnswer(message);
        }

        private void OnItemOperationError(AppChangingItemsMessage message, Error error)
        {
            if (error.ErrorCode == ErrorCode.DMarketTokenExpired)
            {
                RetryWithRefreshToken(message);
            }
            else
            {
                var answer = new AppChangingItemsAnswerMessage();
                var errorMessage = GetErrorMessage(error);
                answer.Response = new ItemsChangingResponse(false, errorMessage);
                SendAnswer(answer, message);
            }
        }

        private string GetErrorMessage(Error error)
        {
            return Controller.ApiErrorHelper.GetErrorMessage(error.ErrorCode);
        }

        #region refresh token logic
        private void RetryWithRefreshToken(AppChangingItemsMessage message)
        {
            RefreshMarketToken(
                            () => OnItemChangingMsg(message),
                            error =>
                            {
                                var answer = new AppChangingItemsAnswerMessage();
                                answer.Response = new ItemsChangingResponse(false, error);
                                SendAnswer(answer, message);
                            });
        }

        private void RefreshMarketToken(Action success, Action<string> faill)
        {
            DMarketApi.GetMarketRefreshToken(Model.MakretRefreshToken,
                (responce, requestParam) =>
                {
                    Model.MarketToken = responce.token;
                    Model.MakretRefreshToken = responce.refreshToken;
                    Model.SetChanges();
                    success.SafeRaise();
                },
                error =>
                {
                    faill.SafeRaise(GetErrorMessage(error));
                });
        }
        #endregion

        private void OnLoadDMarketDataMsg(AppLoadDMarketDataMessage message)
        {
            if (!IsCanMakeDMarketOperation(message))
            {
                return;
            }
            DMarketApi.GetInMarketInventory(Model.MarketToken,
                (responce, request) =>
                {
                    PlayerInfo playerInfo = GetPlayer(message.ConnectionId);

                    PlayerInventoryInfo inventory = GetInventory(message.ConnectionId);
                    playerInfo.Inventory = inventory;
                    inventory.RemoveAllDMakretItems();

                    foreach (var item in responce.Items)
                    {
                        GameItemType itemType = _dMarketConverter.GetItemType(item.classId);
                        long worldId = _dMarketConverter.GetWorldId(item.assetId);

                        PlayerItemInfo dmarketItem = new PlayerItemInfo(itemType, worldId, true);
                        inventory.AddItem(dmarketItem);
                    } 

                    Storage.Change(playerInfo);

                    SendDmarketDataUpdateAnswer(message);
                },
                error =>
                {
                    if (error.ErrorCode == ErrorCode.DMarketTokenExpired)
                    {
                        RefreshMarketToken(
                            () => OnLoadDMarketDataMsg(message),
                            errorParam => SendLoadDMarketLoadDataError(message, errorParam));
                        return;
                    }
                    SendLoadDMarketLoadDataError(message, error.ErrorMessage);
                });
        }

        private void SendLoadDMarketLoadDataError(AppLoadDMarketDataMessage message,
            string error)
        {
            var answer = new AppLoadDMarketAnswerMessage();
            answer.Responce.ErrorText = error;
            SendAnswer(answer, message);
        }

        private void SendDmarketDataUpdateAnswer(AppMessageBase sender)
        {
            var answer = new AppLoadDMarketAnswerMessage();
            answer.Responce.Inventory = GetInventory(sender.ConnectionId);
            SendAnswer(answer, sender);
        }
        
        private bool IsCanMakeDMarketOperation(AppMessageBase message)
        {
            var answer = new AppChangingItemsAnswerMessage();
            if (String.IsNullOrEmpty(Model.MarketToken))
            {
                answer.Error = NetMsgErrorType.DMarketError;
                answer.Response.ErrorText = "Market token is empty";
                SendAnswer(answer, message);
                return false;
            }
            var itemOperationMsg = message as AppChangingItemsMessage;
            if (itemOperationMsg != null)
            {
                var playerInfo = GetPlayer(message.ConnectionId);
                if (playerInfo == null)
                {
                    var error = new Error();
                    error.ErrorMessage = string.Format("Cannot find player {0}. " +
                        "Need reload game", Model.ConIdToUserName[message.ConnectionId]);
                    error.ErrorCode = ErrorCode.AssetNotFound;
                    OnItemOperationError(itemOperationMsg, error);
                    return false;
                }
            }
            return true;
        }
    }
}