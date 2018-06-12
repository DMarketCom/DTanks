using Networking.Msg;
using PlayerData;
using Shop.Domain;
using DMarketSDK.IntegrationAPI;

namespace GameServer.Commands
{
    public class ServerItemsOperationCommand : ServerAppCommandBase
    {
        private ServerApi DMarketApi { get { return Controller.DMarkteServerApi; } }
        
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
            PlayerItemInfo itemInfo = playerInfo.Inventory.GetItem(message.WorldId);
            GameItemType itemType = itemInfo.ItemType;;

            int price = GetPrice(itemType);

            ItemsChangingResponse response;
            if (playerInfo.Inventory.Coins >= price)
            {
                PlayerItemInfo newItemInfo = new PlayerItemInfo(itemType, Storage.GetUniqueWorldId());
                playerInfo.Inventory.AddItem(newItemInfo);
                playerInfo.Inventory.Coins -= price;

                Storage.Change(playerInfo);

                response = new ItemsChangingResponse(true);
            }
            else
            {
                response = new ItemsChangingResponse(false, "Not enough coins");
            }

            AppChangingItemsAnswerMessage answer = new AppChangingItemsAnswerMessage();
            SendAnswer(answer, message);
        }
        
        private void OnItemSellMsg(AppChangingItemsMessage message)
        {
            var playerInfo = GetPlayer(message.ConnectionId);
            var itemType = playerInfo.Inventory.GetItem(message.WorldId).ItemType;
            var price = GetPrice(itemType);
            var answer = new AppChangingItemsAnswerMessage();
            playerInfo.Inventory.Coins += price;
            playerInfo.Inventory.RemoveItem(message.WorldId);
            Storage.Change(playerInfo);
            answer.Response = new ItemsChangingResponse(true);
            SendAnswer(answer, message);
        }

        private void OnItemEquipMsg(AppChangingItemsMessage message)
        {
            PlayerInfo playerInfo = GetPlayer(message.ConnectionId);
            PlayerItemInfo item = playerInfo.Inventory.GetItem(message.WorldId);

            playerInfo.Inventory.EquipItem(item);
            Storage.Change(playerInfo);

            var answer = new AppChangingItemsAnswerMessage
            {
                Response = new ItemsChangingResponse(true)
            };

            SendAnswer(answer, message);
        }

        private void OnItemDevTestAdd(AppChangingItemsMessage message)
        {
            var playerData = GetPlayer(message.ConnectionId);
            var itemType = message.ItemType;
            var newItem = new PlayerItemInfo(itemType,
                    Storage.GetUniqueWorldId());
            playerData.Inventory.Items.Add(newItem);
            Storage.Change(playerData);
            var answer = new AppChangingItemsAnswerMessage();
            answer.Response = new ItemsChangingResponse(true);
            SendAnswer(answer, message);
        }

        private int GetPrice(GameItemType itemType)
        {
            return 100;
        }
    }
}