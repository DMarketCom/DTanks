using DMarketSDK.IntegrationAPI;
using TankGame.Domain.GameItem;
using TankGame.Domain.PlayerData;
using TankGame.Inventory.Domain;
using TankGame.Network.Messages;

namespace TankGame.GameServer.Commands.Inventory
{
    public class ServerItemsOperationCommand : ServerAppCommandBase
    {
        private ServerApi DMarketApi { get { return Controller.DMarketServerApi; } }
        
        protected override void OnAppMsgReceived(AppMessageBase message)
        {
            switch (message.Type)
            {
                case AppMsgType.ItemChangingRequest:
                    OnItemChangingMsg(message as AppChangingItemsMessage);
                    break;
            }
        }

        private void OnItemChangingMsg(AppChangingItemsMessage message)
        {
            switch (message.ActionType)
            {
                case ItemActionType.Buy:
                    OnItemBuyMsg(message);
                    break;
                case ItemActionType.Sell:
                    OnItemSellMsg(message);
                    break;
                case ItemActionType.Equip:
                    OnItemEquipMsg(message);
                    break;
                case ItemActionType.DevTestAdd:
                    OnItemDevTestAdd(message);
                    break;
            }
        }

        private void OnItemBuyMsg(AppChangingItemsMessage message)
        {
            PlayerInfo playerInfo = GetPlayer(message.ConnectionId);
            var response = new ItemsChangingResponse();
            for (var i = 0; i < message.ItemsCount; i++)
            {
                var worldId = message.GetWorldId(i);
                PlayerItemInfo itemInfo = playerInfo.Inventory.GetItem(worldId);
                GameItemType itemType = itemInfo.ItemType;
                int price = GetPrice(itemType);

                if (playerInfo.Inventory.Coins >= price)
                {
                    PlayerItemInfo newItemInfo = new PlayerItemInfo(itemType, Storage.GetUniqueWorldId());
                    playerInfo.Inventory.AddItem(newItemInfo);
                    playerInfo.Inventory.Coins -= price;      
                }
                else
                {
                    response.ErrorText = "Not enough coins";
                    break;
                }
            }
            Storage.Change(playerInfo);
            AppChangingItemsAnswerMessage answer = new AppChangingItemsAnswerMessage(response);
            SendMessageToClient(answer, message.ConnectionId);
        }
        
        private void OnItemSellMsg(AppChangingItemsMessage message)
        {
            var playerInfo = GetPlayer(message.ConnectionId);
            for (var i = 0; i < message.ItemsCount; i++)
            {
                var worldId = message.GetWorldId(i);
                var itemType = playerInfo.Inventory.GetItem(worldId).ItemType;
                var price = GetPrice(itemType);
                playerInfo.Inventory.Coins += price;
                playerInfo.Inventory.RemoveItem(worldId);
            }
            Storage.Change(playerInfo);
            var answer = new AppChangingItemsAnswerMessage(new ItemsChangingResponse());
            SendMessageToClient(answer, message.ConnectionId);
        }

        private void OnItemEquipMsg(AppChangingItemsMessage message)
        {
            PlayerInfo playerInfo = GetPlayer(message.ConnectionId);
            for (var i = 0; i < message.ItemsCount; i++)
            {
                var worldId = message.GetWorldId(i);
                var item = playerInfo.Inventory.GetItem(worldId);
                playerInfo.Inventory.EquipItem(item);
            }
            Storage.Change(playerInfo);
            var answer = new AppChangingItemsAnswerMessage(new ItemsChangingResponse());
            SendMessageToClient(answer, message.ConnectionId);
        }

        private void OnItemDevTestAdd(AppChangingItemsMessage message)
        {
            var playerData = GetPlayer(message.ConnectionId);
            var itemType = message.GetItemType(0);
            var newItem = new PlayerItemInfo(itemType,
                    Storage.GetUniqueWorldId());
            playerData.Inventory.Items.Add(newItem);
            Storage.Change(playerData);
            var answer = new AppChangingItemsAnswerMessage(new ItemsChangingResponse());
            SendMessageToClient(answer, message.ConnectionId);
        }

        private int GetPrice(GameItemType itemType)
        {
            return 100;
        }
    }
}