using System.Collections.Generic;
using Game.PickupItems;
using Game.Tank;
using TankGame.GameClient.Commands;
using TankGame.Network.Client;
using TankGame.Network.Messages;
using TankGame.UI.Forms;
using UnityEngine;

namespace Game.States.Online
{
    public class GameIdleOnlineState : GameIdleStateBase
    {
        private IGameClient Client { get { return Controller.Client; } }
        private PickUpItemsManager PickUpManager { get { return Context.PickUpManager; } }

        private readonly Dictionary<int, ITank> _opponnets = new Dictionary<int, ITank>();

        public override void Start(object[] args = null)
        {
            base.Start(args);
            var existingOpponents = args[0] as List<GameBattlePlayerInfo>;
            existingOpponents.ForEach(CreateAndRespawnOpponent);
            Client.GameMsgReceived += OnMsgReceived;
            Player.Moved += OnPlayerMoved;
            Player.Weapon.Fire += OnPlayerFire;
            PickUpManager.UnitPickUpItem += OnPlayerPickUpItem;
        }

        public override void Finish()
        {
            base.Finish();
            Client.GameMsgReceived -= OnMsgReceived;
            Player.Moved -= OnPlayerMoved;
            Player.Weapon.Fire -= OnPlayerFire;
            PickUpManager.UnitPickUpItem -= OnPlayerPickUpItem;
            _opponnets.Clear();
        }

        protected override void OnPlayerDied(ITank tank)
        {
            Client.Send(new UnitDestroyMessage());
            ApplyCommand(new ShowGameOverPopUpCommand());
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
            var message = new UnitPositionMessage(tank.Position, tank.Rotation);
            Client.Send(message);
        }
        
        private void OnPlayerFire(Vector3 direction, Vector3 target, float force)
        {
            var message = new BulletStartedMsg(target, force);
            Client.Send(message);
        }

        private void OnMsgReceived(GameMessageBase message)
        {
            switch (message.Type)
            {
                case GameMsgType.OpponentRespawn:
                    OnOpponentTankRespawn(message as TankRespawnMsg);
                    break;
                case GameMsgType.UnitPosition:
                    OnUnitMoved(message as UnitPositionMessage);
                    break;
                case GameMsgType.UnitDestroy:
                    OnOpponentDied(message as UnitDestroyMessage);
                    break;
                case GameMsgType.BulletStarted:
                    OnOpponentBulletStarted(message as BulletStartedMsg);
                    break;
                case GameMsgType.PickupGameItem:
                    OnPickupGameItem(message as PickUpGameItemMsg);
                    break;
                case GameMsgType.CreateDropItem:
                    OnDropItem(message as CreateDropItemMsg);
                    break;
            }
        }

        private void OnOpponentTankRespawn(TankRespawnMsg message)
        {
            CreateAndRespawnOpponent(message.Opponent);
        }

        private void CreateAndRespawnOpponent(GameBattlePlayerInfo opponentUnit)
        {
            if (!_opponnets.ContainsKey(opponentUnit.UnitId))
            {
                var newOpponent = Context.CreateTank(false, Model.Mode, opponentUnit.EquippedItemsTypes);
                _opponnets.Add(opponentUnit.UnitId, newOpponent);
                Controller.Opponents.Add(newOpponent);
            }
            _opponnets[opponentUnit.UnitId].Respawn(opponentUnit.Position);
        }

        private void OnOpponentBulletStarted(BulletStartedMsg message)
        {
            _opponnets[message.UnitId].Weapon.MakeFire(message.Target, message.Force);
        }

        private void OnOpponentDied(UnitDestroyMessage message)
        {
            (_opponnets[message.UnitId] as INetworkTank).Broke();
        }

        private void OnUnitMoved(UnitPositionMessage message)
        {
            _opponnets[message.UnitId].Rotation = message.RotationY;
            UpdateOpponentTankPos(message.UnitId, message.Position);
        }

        private void UpdateOpponentTankPos(int unitId, Vector3 pos)
        {
            var tank = _opponnets[unitId];
            const float kMinDistanceForUsingTween = 0.05f;
            const float kMaxDistanceForUsingTween = 3f;
            var distanceDelay = Vector3.Distance(tank.Position, pos);
            if (distanceDelay > kMinDistanceForUsingTween 
                && distanceDelay < kMaxDistanceForUsingTween)
            {
                (tank as INetworkTank).Move(pos, 0.5f);
            }
            else
            {
                tank.Position = pos;
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

        private void OnPlayerPickUpItem(int unitId, PickupItem item)
        {
            if (Player.Unit.UnitId == unitId && Player.IsAlive)
            {
                var itemType = item.ItemType;
                var dropInfo = new DropItemInfo { WorldId = item.WorldId, CatalogId = itemType };
                Client.Send(new PickUpGameItemMsg { DropInfo = dropInfo });

                string messageText = string.Format("You picked up {0}", itemType);

                SimpleMessageForm notificationMessage = View.CreateNotificationForm();
                notificationMessage.Init(messageText);

                ApplyCommand(new ShowNotificationFormCommand(notificationMessage, 2f));
            }
        }
    }
}