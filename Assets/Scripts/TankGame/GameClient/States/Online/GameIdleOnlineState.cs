using System.Collections.Generic;
using Game.Bullet;
using Game.PickupItems;
using Game.Tank;
using Game.Units.Components;
using Networking.Client;
using Networking.Msg;
using TankGame.Forms;
using TankGame.GameClient.Commands;
using UnityEngine;

namespace Game.States.Online
{
    public class GameIdleOnlineState : GameIdleStateBase
    {
        private IGameClient Client { get { return Controller.Client; } }
        private PickUpItemsManager PickUpManager { get { return Context.PickUpManager; } }

        private readonly Dictionary<int, ITank> _oponents = new Dictionary<int, ITank>();

        public override void Start(object[] args = null)
        {
            base.Start(args);
            var alreadyExistingOponents = args[0] as List<GameBattlePlayerInfo>;
            alreadyExistingOponents.ForEach(oponent => CreateAndRespawnOponent(oponent));
            Client.GameMsgReceived += OnMsgReceived;
            Player.Moved += OnPlayerMoved;
            (Player.Weapon as IWeaponOutsideComponent).MakedFire += OnPlayerMakedFire;
            PickUpManager.UnitPicked += OnPickedItem;
            ScheduledUpdate(2f, true);
        }

        public override void Finish()
        {
            base.Finish();
            Client.GameMsgReceived -= OnMsgReceived;
            Player.Moved -= OnPlayerMoved;
            (Player.Weapon as IWeaponOutsideComponent).MakedFire -= OnPlayerMakedFire;
            PickUpManager.UnitPicked -= OnPickedItem;
            _oponents.Clear();
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();
            if (Player.IsAlive)
            {
                Client.Send(new TankStateUpdateMsg(Player.Pos));
            }
        }

        protected override void OnPlayerDied(ITank tank)
        {
            Client.Send(new TankDiedMsg());
            ApplyCommand(new ShowGameOverPopUpCommand());
        }

        protected override void OnOponentHitted(GameUnitBase unit, IBullet bullet)
        {
        }

        protected override void OnBackClick()
        {
            if (Player.IsAlive)
            {
                (Player as INetworkTank).Broke();
            }
        }

        private void OnPlayerMoved(ITank tank)
        {
            var message = new UnitMovedMsg(tank.Pos, tank.Rotation);
            Client.Send(message);
        }
        
        private void OnPlayerMakedFire(IWeaponOutsideComponent weapon, Vector3 target, float force)
        {
            var message = new BulletStartedMsg(target, force);
            Client.Send(message);
        }

        private void OnMsgReceived(GameMessageBase message)
        {
            switch (message.Type)
            {
                case GameMsgType.OponentRespawn:
                    OnOponentTankRespawn(message as TankRespawnMsg);
                    break;
                case GameMsgType.UnitMoved:
                    OnUnitMoved(message as UnitMovedMsg);
                    break;
                case GameMsgType.Died:
                    OnOponentDied(message as TankDiedMsg);
                    break;
                case GameMsgType.BulletStarted:
                    OnOponentBulletStarted(message as BulletStartedMsg);
                    break;
                case GameMsgType.TankStateUpdate:
                    OnOpponentUpdateState(message as TankStateUpdateMsg);
                    break;
                case GameMsgType.PickupGameItem:
                    OnPickupGameItem(message as PickUpGameItemMsg);
                    break;
                case GameMsgType.CreateDropItem:
                    OnDropItem(message as CreateDropItemMsg);
                    break;
            }
        }

        private void OnOponentTankRespawn(TankRespawnMsg message)
        {
            CreateAndRespawnOponent(message.Oponent);
        }

        private void CreateAndRespawnOponent(GameBattlePlayerInfo oponentInfo)
        {
            if (!_oponents.ContainsKey(oponentInfo.UnitId))
            {
                var newOponent = Context.CreateTank(false, Model.Mode, oponentInfo.EquippedItemsTypes);
                _oponents.Add(oponentInfo.UnitId, newOponent);
                Controller.Opponents.Add(newOponent);
            }
            _oponents[oponentInfo.UnitId].Respawn(oponentInfo.Position);
        }

        private void OnOponentBulletStarted(BulletStartedMsg message)
        {
            _oponents[message.UnitId].Weapon.MakeFire(message.Target, message.Force);
        }

        private void OnOponentDied(TankDiedMsg message)
        {
            (_oponents[message.UnitId] as INetworkTank).Broke();
        }

        private void OnUnitMoved(UnitMovedMsg message)
        {
            _oponents[message.UnitId].Rotation = message.RotY;
            UpdateOpponentTankPos(message.UnitId, message.Pos);
        }

        private void OnOpponentUpdateState(TankStateUpdateMsg message)
        {
            UpdateOpponentTankPos(message.UnitId, message.Pos);
        }

        private void UpdateOpponentTankPos(int unitId, Vector3 pos)
        {
            var tank = _oponents[unitId];
            const float kMinDistanceForUsingTween = 0.05f;
            const float kMaxDistanceForUsingTween = 3f;
            var distanceDelay = Vector3.Distance(tank.Pos, pos);
            if (distanceDelay > kMinDistanceForUsingTween 
                && distanceDelay < kMaxDistanceForUsingTween)
            {
                (tank as INetworkTank).Move(pos, 0.5f);
            }
            else
            {
                tank.Pos = pos;
            }
        }

        private void OnPickupGameItem(PickUpGameItemMsg message)
        {
            PickUpManager.DestroyItem(message.DropInfo.WorldId);
        }

        private void OnDropItem(CreateDropItemMsg message)
        {
            var dropItem = message.DropInfo;
            PickUpManager.DropItem(dropItem.CatalogId, dropItem.WorldId,
                dropItem.Pos);
        }

        private void OnPickedItem(int unitId, PickupItem item)
        {
            if (Player.Unit.UnitID == unitId && Player.IsAlive)
            {
                var itemType = item.ItemType;
                var dropInfo = new DropItemInfo { WorldId = item.WorldId, CatalogId = itemType };
                Client.Send(new PickUpGameItemMsg { DropInfo = dropInfo });

                string messageText = string.Format("You picked up {0}", itemType);

                SimpleMessageForm notificationMessage = View.NotificationForm;
                notificationMessage.Init(messageText);

                ApplyCommand(new ShowNotificationFormCommand(notificationMessage, 2f, 0.3f));
            }
        }
    }
}