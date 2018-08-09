using System.Collections.Generic;
using TankGame.Domain.GameItem;
using TankGame.Network.Messages;
using UnityEngine;

namespace TankGame.GameServer.Commands.Battle
{
    public class ServerBattleUserRegistrationCommand : ServerBattleCommandBase
    {
        public override void Start()
        {
            base.Start();
            Server.Connected += OnConnected;
            Server.Disconnected += OnDisconnected;
        }

        protected override void Finish()
        {
            base.Finish();
            Server.Connected -= OnConnected;
            Server.Disconnected -= OnDisconnected;
        }

        protected override void OnGameMsgReceived(GameMessageBase message)
        {
            var unitMessage = message as GameUnitMessageBase;
            if (unitMessage != null)
            {
                unitMessage.UnitId = Model.UnitsInBattle[unitMessage.ClientId].UnitId;
            }

            var connectMessage = message as ConnectToBattleRequestMsg;
            if (connectMessage != null)
            {
                OnConnectedToBattleRequest(connectMessage);
            }
        }

        private void OnConnected(int conId)
        {
            Model.UnitsInBattle.Add(conId, new GameBattlePlayerInfo());
        }

        private void OnDisconnected(int conId)
        {
            var diedMsg = new TankDiedMsg {ClientId = conId};
            Server.SendToAllExcept(diedMsg, conId);
            Model.UnitsInBattle.Remove(conId);
        }

        private void OnConnectedToBattleRequest(ConnectToBattleRequestMsg message)
        {
            string userName = Model.GetUserNameByConnectionId(message.ClientId);
            List<GameItemType> equippedItems = Storage.Get(userName).Inventory.GetEquippedItemsTypes();

            GameBattlePlayerInfo battlePlayerInfo = new GameBattlePlayerInfo(message.ClientId, userName, GetFreeRespawnPoint(), equippedItems);

            Model.UnitsInBattle[message.ClientId] = battlePlayerInfo;

            SendBattleStateToNewPlayer(message, battlePlayerInfo);
            SendToAllAboutNewPlayer(message, battlePlayerInfo);
        }

        private void SendBattleStateToNewPlayer(ConnectToBattleRequestMsg message,
            GameBattlePlayerInfo newPlayer)
        {
            var answer = new ConnectToBattleAnswerMsg();
            answer.IsCanConnect = true;
            answer.Player = newPlayer;
            answer.Opponents = new List<GameBattlePlayerInfo>();
            foreach (var unitId in Model.UnitsInBattle.Keys)
            {
                if (unitId != message.ClientId && Model.UnitsInBattle[unitId].IsAlive)
                {
                    answer.Opponents.Add(Model.UnitsInBattle[unitId]);
                }
            }
            answer.DroppedItems = Model.ItemsInField;
            Server.SendToPlayer(answer, message.ClientId);
        }

        private void SendToAllAboutNewPlayer(ConnectToBattleRequestMsg message,
            GameBattlePlayerInfo player)
        {
            var newUnitMessage = new TankRespawnMsg();
            newUnitMessage.Opponent = player;
            newUnitMessage.ClientId = player.UnitId;
            Server.SendToAllExcept(newUnitMessage, newUnitMessage.ClientId);
        }

        private Vector3 GetFreeRespawnPoint()
        {
            return Controller.BattleField.GetSpawnPoint();
        }
    }
}