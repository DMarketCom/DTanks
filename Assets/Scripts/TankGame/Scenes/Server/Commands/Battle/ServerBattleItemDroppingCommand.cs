using TankGame.Domain.GameItem;
using TankGame.Domain.PlayerData;
using TankGame.Network.Messages;
using UnityEngine;

namespace TankGame.GameServer.Commands.Battle
{
    public class ServerBattleItemDroppingCommand : ServerBattleCommandBase
    {
        private float kMinDropInterval = 5f;
        private float kMaxDropInterval = 15f;
        private int kMaxItemsInField = 10;
        
        private int _lastItemSpawnPointIndex = 0;

        public override void Start()
        {
            base.Start();
            DropItemInField();
        }

        private void DropItemInField()
        {
            if (Model.ItemsInField.Count < kMaxItemsInField)
            {
                if (_lastItemSpawnPointIndex >= Controller.BattleField.TotalPointsForItemSpawn)
                {
                    _lastItemSpawnPointIndex = 0;
                }
                if (Model.SpawnPointIsEmpty(_lastItemSpawnPointIndex))
                {
                    var dropPos = Controller.BattleField.GetPointForItem(_lastItemSpawnPointIndex);
                    DropItem(GameItemCategory.Helmet, dropPos, _lastItemSpawnPointIndex);
                }
                _lastItemSpawnPointIndex++;
            }
            var timeForDropItem = UnityEngine.Random.Range(kMinDropInterval, kMaxDropInterval);
            ScheduledUpdate(timeForDropItem);
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();
            DropItemInField();
        }

        protected override void OnGameMsgReceived(GameMessageBase message)
        {
            switch (message.Type)
            {
                case GameMsgType.Died:
                    OnTankDied(message as TankDiedMsg);
                    break;
                case GameMsgType.PickupGameItem:
                    OnPickUpItem(message as PickUpGameItemMsg);
                    break;
            }
        }

        private void OnTankDied(TankDiedMsg message)
        {
            DropItem(GameItemCategory.Skin, Model.UnitsInBattle[message.ClientId].Position);
        }

        private void OnPickUpItem(PickUpGameItemMsg message)
        {
            var item = message.DropInfo;
            Model.RemoveItem(item.WorldId);

            string userName = Model.GetUserNameByConnectionId(message.ClientId);
            PlayerInfo playerData = Storage.Get(userName);

            PlayerItemInfo itemInfo = new PlayerItemInfo(item.CatalogId, item.WorldId);

            playerData.Inventory.AddItem(itemInfo);
            Storage.Change(playerData);

            Server.SendToAllExcept(message, message.ClientId);
        }

        private void DropItem(GameItemCategory category, Vector3 pos, int itemSpawnPointIndex = -1)
        {
            var dropItem = new DropItemInfo()
            {
                CatalogId = GameItemTypeExtensions.GetRandomItem(category),
                Pos = pos,
                ItemSpawnPointIndex = itemSpawnPointIndex,
                WorldId = Storage.GetUniqueWorldId()
            };
            Model.AddItem(dropItem);
            var dropItemMessage = new CreateDropItemMsg() { DropInfo = dropItem };
            Server.SendToAll(dropItemMessage);
        }
    }
}