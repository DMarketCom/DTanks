using System.Collections.Generic;
using System.Linq;
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
            Server.Disconnected += OnDisconnected;
        }

        protected override void Finish()
        {
            base.Finish();
            Server.Disconnected -= OnDisconnected;
        }

        protected override void OnGameMsgReceived(GameMessageBase message)
        {
            var unitMessage = message as UnitMessageBase;
            if (unitMessage != null)
            {
                unitMessage.UnitId = Model.GetBattlePlayer(unitMessage.ClientId).UnitId;
            }

            var connectMessage = message as ConnectToBattleRequestMsg;
            if (connectMessage != null)
            {
                OnConnectedToBattleRequest(connectMessage);
            }
        }

        private void OnDisconnected(int connectionId)
        {
            if(Model.IsUserInBattle(connectionId))
            {
                Model.RemoveBattlePlayer(connectionId);
                var destroyMessage = new UnitDestroyMessage { ClientId = connectionId };
                Server.SendToAllExcept(destroyMessage, connectionId);
            }
        }

        private void OnConnectedToBattleRequest(ConnectToBattleRequestMsg message)
        {
            int connectionId = message.ClientId;
            string userName = Model.GetUserNameByConnectionId(connectionId);
            List<GameItemType> equippedItems = Storage.Get(userName).Inventory.GetEquippedItemsTypes();

            GameBattlePlayerInfo battlePlayerInfo = new GameBattlePlayerInfo(connectionId, userName, GetFreeRespawnPoint(), equippedItems);

            if (Model.IsUserInBattle(connectionId)) // TODO: need separate EnterBattle and RespawnTank logic.
            {
                Model.RemoveBattlePlayer(connectionId);
            }

            Model.AddBattlePlayer(message.ClientId, battlePlayerInfo);

            SendBattleState(message.ClientId);
            SendToAllAboutNewPlayer(battlePlayerInfo);
        }

        private void SendBattleState(int playerConnectionId)
        {
            var battlePlayer = Model.GetBattlePlayer(playerConnectionId);
            var allPlayersConnection = Model.GetBattlePlayersConnections();

            var answer = new ConnectToBattleAnswerMsg
            {
                IsCanConnect = true,
                Player = battlePlayer,
                Opponents = new List<GameBattlePlayerInfo>(),
                DroppedItems = Model.ItemsInField
            };

            foreach (var connectionId in allPlayersConnection.Where(c => c != playerConnectionId))
            {
                GameBattlePlayerInfo gameBattlePlayerInfo = Model.GetBattlePlayer(connectionId);
                if (gameBattlePlayerInfo.IsAlive)
                {
                    answer.Opponents.Add(gameBattlePlayerInfo);
                }
            }

            Server.SendToPlayer(answer, playerConnectionId);
        }

        private void SendToAllAboutNewPlayer(GameBattlePlayerInfo player)
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