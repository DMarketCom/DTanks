using System;
using System.Collections.Generic;
using SHLibrary.Logging;
using TankGame.Network.Messages;
using UnityEngine.Networking;

namespace TankGame.Network
{
    public static class NetworkingUtil
    {
        private static readonly Dictionary<GameMsgType, 
            Func<NetworkMessage, GameMessageBase>> GameMsgHandlers;

        private static readonly Dictionary<AppMsgType, 
            Func<NetworkMessage, AppMessageBase>> AppMsgHandlers;

        static NetworkingUtil()
        {
            GameMsgHandlers = new Dictionary<GameMsgType, Func<NetworkMessage, GameMessageBase>>
            {
                {GameMsgType.UnitPosition, msg => msg.ReadMessage<UnitPositionMessage>()},
                {GameMsgType.BulletStarted, msg => msg.ReadMessage<BulletStartedMsg>()},
                {GameMsgType.UnitDestroy, msg => msg.ReadMessage<UnitDestroyMessage>()},
                {GameMsgType.ConnectToBattleRequest, msg => msg.ReadMessage<ConnectToBattleRequestMsg>()},
                {GameMsgType.ConnectToBattleAnswer, msg => msg.ReadMessage<ConnectToBattleAnswerMsg>()},
                {GameMsgType.OpponentRespawn, msg => msg.ReadMessage<TankRespawnMsg>()},
                {GameMsgType.CreateDropItem, msg => msg.ReadMessage<CreateDropItemMsg>()},
                {GameMsgType.PickupGameItem, msg => msg.ReadMessage<PickUpGameItemMsg>()}
            };

            AppMsgHandlers = new Dictionary<AppMsgType, Func<NetworkMessage, AppMessageBase>>
            {
                {AppMsgType.Login, msg => msg.ReadMessage<LoginMessage>()},
                {AppMsgType.LoginAnswer, msg => msg.ReadMessage<LoginAnswerMessage>()},
                {AppMsgType.Logout, msg => msg.ReadMessage<LogoutMessage>()},
                {AppMsgType.Registration, msg => msg.ReadMessage<RegistrationMessage>()},
                {AppMsgType.RegistrationAnswer, msg => msg.ReadMessage<RegistrationAnswerMessage>()},
                {AppMsgType.UpdatePlayerDataAnswer, msg => msg.ReadMessage<AppUpdatePlayerDataAnswerMessage>()},
                {AppMsgType.LoadDMarketDataRequest, msg => msg.ReadMessage<AppLoadDMarketDataMessage>()},
                {AppMsgType.LoadDMarketDataAnswer, msg => msg.ReadMessage<AppLoadDMarketAnswerMessage>()},
                {AppMsgType.ItemChangingRequest, msg => msg.ReadMessage<AppChangingItemsMessage>()},
                {AppMsgType.ItemChangingAnswer, msg => msg.ReadMessage<AppChangingItemsAnswerMessage>()},
                {AppMsgType.GetGameTokenRequest, msg => msg.ReadMessage<AppGetGameTokenMessage>()},
                {AppMsgType.GetGameTokenAnswer, msg => msg.ReadMessage<AppGetGameTokenAnswerMessage>()},
                {AppMsgType.UpdateMarketToken, msg => msg.ReadMessage<AppUpdateMarketTokenMessage>()},
                {AppMsgType.DMarketTransactionCompleted, msg => msg.ReadMessage<AppDMarketTransactionMessage>()},
                {AppMsgType.LoadInventoryBasicIntegration, msg => msg.ReadMessage<InventoryBasicIntegrationMessage>()},
                {AppMsgType.LoadInventoryBasicIntegrationAnswer, msg => msg.ReadMessage<InventoryBasicIntegrationAnswerMessage>()},
                {AppMsgType.UnloadInventoryBasicIntegration, msg => msg.ReadMessage<UnloadInventoryBasicIntegrationMessage>()}
            };
        }

        public static GameMessageBase ReadGameMessage(NetworkMessage msg)
        {
            var type = (GameMsgType)msg.msgType;
            if (GameMsgHandlers.ContainsKey(type))
            {
                var result = GameMsgHandlers[type](msg);
                result.ClientId = msg.conn.connectionId;
                return result;
            }

            DevLogger.Error("Have no receive method for " + type, DTanksLogChannel.Network);
            return null;
        }

        public static AppMessageBase ReadAppMessage(NetworkMessage msg)
        {
            var type = (AppMsgType)msg.msgType;
            if (AppMsgHandlers.ContainsKey(type))
            {
                var result = AppMsgHandlers[type](msg);
                result.ConnectionId = msg.conn.connectionId;
                return result;
            }

            DevLogger.Error("Have no receive method for " + type, DTanksLogChannel.Network);
            return null;
        }

        public static List<short> GetAllShortCodesForAppMessages()
        {
            return GetAllNumbersFromRange(
                (short)AppMsgType.Login, (short)AppMsgType.Highest);
        }

        public static List<short> GetAllShortCodesForGameMessages()
        {
            return GetAllNumbersFromRange((short)GameMsgType.UnitPosition,
                (short)GameMsgType.Highest);
        }

        private static List<short> GetAllNumbersFromRange(short from, short to)
        {
            var result = new List<short>();
            for (var i = from; i < to; i++)
            {
                result.Add(i);
            }
            return result;
        }
    }
}