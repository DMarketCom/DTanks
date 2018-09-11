using System.Collections.Generic;
using DMarketSDK.IntegrationAPI;
using SHLibrary.Logging;
using TankGame.Domain.GameItem;
using TankGame.Domain.PlayerData;
using TankGame.GameServer.Commands.DMarket.Domain;
using TankGame.Inventory.Domain;
using TankGame.Network.Messages;

namespace TankGame.GameServer.Commands.DMarket
{
    public class ServerDMarketIntegrationCommand : ServerDMarketCommandBase
    {
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
                case AppMsgType.LoadDMarketDataRequest:
                    OnLoadDMarketDataMsg(message as AppLoadDMarketDataMessage);
                    break;
                case AppMsgType.LoadInventoryBasicIntegration:
                    OnLoadInventoryBasicIntegration(message as InventoryBasicIntegrationMessage);
                    break;
                case AppMsgType.UnloadInventoryBasicIntegration:
                    OnLeaveInventory(message as UnloadInventoryBasicIntegrationMessage);
                    break;
            }
        }

        private void OnLeaveInventory(UnloadInventoryBasicIntegrationMessage message)
        {
            int connectionId = message.ConnectionId;
            if (!IsCanMakeDMarketOperation(connectionId))
            {
                return;
            }

            PlayerInfo playerInfo = GetPlayer(connectionId);
            PlayerInventoryInfo inventory = playerInfo.Inventory;

            List<ItemInfo> movingItems = new List<ItemInfo>();
            foreach (var playerItemInfo in inventory.Items)
            {
                if (inventory.IsEquipped(playerItemInfo.WorldId))
                {
                    continue;
                }
                
                movingItems.Add(new ItemInfo(playerItemInfo.WorldId, playerItemInfo.ItemType));
            }

            ApplyCommand(new ServerBulkTransferInventoryBasicIntegrationCommand(movingItems, ItemActionType.ToMarket, connectionId));
        }

        private void OnLoadInventoryBasicIntegration(InventoryBasicIntegrationMessage message)
        {
            int connectionId = message.ConnectionId;
            if (!IsCanMakeDMarketOperation(connectionId))
            {
                return;
            }

            DMarketApi.GetInMarketInventory(Model.GetPlayerMarketAccessToken(connectionId),
                (response, request) =>
                {
                    List<ItemInfo> itemsList = new List<ItemInfo>();
                    foreach (var item in response.Items)
                    {
                        GameItemType itemType = DMarketConverter.GetItemType(item.classId);
                        long worldId = DMarketConverter.GetWorldId(item.assetId);

                        ItemInfo dmarketItem = new ItemInfo(worldId, itemType);
                        itemsList.Add(dmarketItem);
                    }

                    if (itemsList.Count == 0)
                    {
                        SendLoadInventoryData(connectionId);
                        return;
                    }

                    ApplyCommand(new ServerBulkTransferInventoryBasicIntegrationCommand(itemsList, ItemActionType.FromMarket, connectionId));
                },
                error =>
                {
                    if (error.ErrorCode == ErrorCode.DMarketTokenExpired)
                    {
                        RefreshMarketToken(connectionId, () => OnLoadInventoryBasicIntegration(message),
                            errorCode => SendLoadInventoryData(connectionId, errorCode));
                        return;
                    }

                    SendLoadInventoryData(connectionId, error.ErrorCode);
                });
        }

        private void OnGetGameTokenMsg(AppGetGameTokenMessage message)
        {
            var answer = new AppGetGameTokenAnswerMessage();
            DMarketApi.GetBasicAccessToken(message.UserId,
                (response, requestParams) =>
                {
                    answer.Response = new DMarketGameTokenResponse(response.token,
                        response.refreshToken);
                    SendMessageToClient(answer, message.ConnectionId);
                },
                error =>
                {
                    DevLogger.Warning(string.Format("Get marketWidget token error: code: {0}, message:" +
                                                    " {1}", error.ErrorCode, error.ErrorMessage));
                    answer.Error = NetworkMessageErrorType.DMarketError;
                    SendMessageToClient(answer, message.ConnectionId);
                });
        }

        private void OnAddMarketTokenMsg(AppUpdateMarketTokenMessage message)
        {
            Model.SetPlayerMarketAccessToken(message.ConnectionId, message.MarketToken);
            Model.SetPlayerMarketRefreshToken(message.ConnectionId, message.RefreshMarketToken);
            Model.SetChanges();
        }

        private void OnLoadDMarketDataMsg(AppLoadDMarketDataMessage message)
        {
            int connectionId = message.ConnectionId;

            if (!IsCanMakeDMarketOperation(connectionId))
            {
                return;
            }

            DMarketApi.GetInMarketInventory(Model.GetPlayerMarketAccessToken(connectionId),
                (response, request) =>
                {
                    PlayerInfo playerInfo = GetPlayer(connectionId);

                    PlayerInventoryInfo inventory = GetInventory(connectionId);
                    playerInfo.Inventory = inventory;
                    inventory.RemoveAllDMakretItems();

                    foreach (var item in response.Items)
                    {
                        GameItemType itemType = DMarketConverter.GetItemType(item.classId);
                        long worldId = DMarketConverter.GetWorldId(item.assetId);

                        PlayerItemInfo dmarketItem = new PlayerItemInfo(itemType, worldId, true);
                        inventory.AddItem(dmarketItem);
                    }

                    Storage.Change(playerInfo);

                    SendDmarketDataUpdateAnswer(connectionId);
                },
                error =>
                {
                    if (error.ErrorCode == ErrorCode.DMarketTokenExpired)
                    {
                        RefreshMarketToken(connectionId, () => OnLoadDMarketDataMsg(message),
                            errorParam => SendLoadDMarketLoadDataError(connectionId, errorParam));
                        return;
                    }

                    SendLoadDMarketLoadDataError(connectionId, error.ErrorCode);
                });
        }

        private void SendLoadDMarketLoadDataError(int connectionId, ErrorCode error)
        {
            var response = new DMarketDataLoadResponse(GetInventory(connectionId), error);
            SendMessageToClient(new AppLoadDMarketAnswerMessage(response), connectionId);
        }

        private void SendLoadInventoryData(int connectionId, ErrorCode error = ErrorCode.None)
        {
            var dataLoadResponse = new DMarketDataLoadResponse(GetInventory(connectionId), error);
            var answerMessage = new InventoryBasicIntegrationAnswerMessage(dataLoadResponse);
            SendMessageToClient(answerMessage, connectionId);
        }

        private void SendDmarketDataUpdateAnswer(int connectionId)
        {
            var response = new DMarketDataLoadResponse(GetInventory(connectionId));
            SendMessageToClient(new AppLoadDMarketAnswerMessage(response), connectionId);
        }

        private bool IsCanMakeDMarketOperation(int connectionId)
        {
            if (string.IsNullOrEmpty(Model.GetPlayerMarketAccessToken(connectionId)))
            {
                var dataLoadResponse = new DMarketDataLoadResponse(GetInventory(connectionId), ErrorCode.EmptyDMarketAccessToken);
                var answer = new AppLoadDMarketAnswerMessage(dataLoadResponse);
                answer.Error = NetworkMessageErrorType.DMarketError;
                SendMessageToClient(answer, connectionId);
                return false;
            }

            return true;
        }
    }
}