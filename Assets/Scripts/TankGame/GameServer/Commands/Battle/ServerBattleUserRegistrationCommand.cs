using Networking.Msg;
using PlayerData;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer.Commands
{
    public class ServerBattleUserRegistrationCommand : ServerBattleCommandBase
    {
        public override void Start()
        {
            base.Start();
            Server.Connected += OnConnected;
            Server.Disconected += OnDisconected;
        }

        protected override void Finish()
        {
            base.Finish();
            Server.Connected -= OnConnected;
            Server.Disconected -= OnDisconected;
        }

        protected override void OnGameMsgReceived(GameMessageBase message)
        {
            if (message is GameUnitMessageBase)
            {
                (message as GameUnitMessageBase).UnitId = Model.UnitsInBattle[message.ClientId].UnitId;
            }
            if (message is ConnectToBattleRequestMsg)
            {
                OnConnectedToBattleRequest(message as ConnectToBattleRequestMsg);
            }
        }

        private void OnConnected(int conId)
        {
            Model.UnitsInBattle.Add(conId, new GameBattlePlayerInfo());
        }

        private void OnDisconected(int conId)
        {
            var diedMsg = new TankDiedMsg();
            diedMsg.ClientId = conId;
            Server.SendToAllExept(diedMsg, conId);
            Model.UnitsInBattle.Remove(conId);
        }

        private void OnConnectedToBattleRequest(ConnectToBattleRequestMsg message)
        {
            string userName = Model.ConIdToUserName[message.ClientId];
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
            answer.Oponents = new List<GameBattlePlayerInfo>();
            foreach (var unitId in Model.UnitsInBattle.Keys)
            {
                if (unitId != message.ClientId && Model.UnitsInBattle[unitId].IsAlive)
                {
                    answer.Oponents.Add(Model.UnitsInBattle[unitId]);
                }
            }
            answer.DropedItems = Model.ItemsInField;
            Server.SendToPlayer(answer, message.ClientId);
        }

        private void SendToAllAboutNewPlayer(ConnectToBattleRequestMsg message,
            GameBattlePlayerInfo player)
        {
            var newUnitMessage = new TankRespawnMsg();
            newUnitMessage.Oponent = player;
            newUnitMessage.ClientId = player.UnitId;
            Server.SendToAllExept(newUnitMessage, newUnitMessage.ClientId);
        }

        private Vector3 GetFreeRespawnPoint()
        {
            return Controller.BattleField.GetSpawnPoint();
        }
    }
}