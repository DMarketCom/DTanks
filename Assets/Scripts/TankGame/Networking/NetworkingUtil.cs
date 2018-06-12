using Networking.Msg;
using SHLibrary.Logging;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Networking
{
    public static class NetworkingUtil
    {
        private static readonly Dictionary<GameMsgType, 
            Func<NetworkMessage, GameMessageBase>> _gameMsgHandlers;

        private static readonly Dictionary<AppMsgType, 
            Func<NetworkMessage, AppMessageBase>> _appMsgHandlers;

        static NetworkingUtil()
        {
            //TODO need write auto code generating script for this monkey part...
            _gameMsgHandlers = new Dictionary<GameMsgType, Func<NetworkMessage, GameMessageBase>>();
            _gameMsgHandlers.Add(GameMsgType.UnitMoved,
                msg => msg.ReadMessage<UnitMovedMsg>());
            _gameMsgHandlers.Add(GameMsgType.BulletStarted,
                msg => msg.ReadMessage<BulletStartedMsg>());
            _gameMsgHandlers.Add(GameMsgType.Died,
                msg => msg.ReadMessage<TankDiedMsg>());
            _gameMsgHandlers.Add(GameMsgType.ConnectToBattleRequest,
                msg => msg.ReadMessage<ConnectToBattleRequestMsg>());
            _gameMsgHandlers.Add(GameMsgType.ConnectToBattleAnswer,
                msg => msg.ReadMessage<ConnectToBattleAnswerMsg>());
            _gameMsgHandlers.Add(GameMsgType.OponentRespawn,
                msg => msg.ReadMessage<TankRespawnMsg>());
            _gameMsgHandlers.Add(GameMsgType.TankStateUpdate,
                msg => msg.ReadMessage<TankStateUpdateMsg>());
            _gameMsgHandlers.Add(GameMsgType.CreateDropItem,
                 msg => msg.ReadMessage<CreateDropItemMsg>());
            _gameMsgHandlers.Add(GameMsgType.PickupGameItem,
                 msg => msg.ReadMessage<PickUpGameItemMsg>());

            _appMsgHandlers = new Dictionary<AppMsgType, Func<NetworkMessage, AppMessageBase>>();
            _appMsgHandlers.Add(AppMsgType.Login,
                msg => msg.ReadMessage<LoginMessage>());
            _appMsgHandlers.Add(AppMsgType.LoginAnswer,
                msg => msg.ReadMessage<LoginAnswerMessage>());
            _appMsgHandlers.Add(AppMsgType.Logout,
                 msg => msg.ReadMessage<LogoutMessage>());
            _appMsgHandlers.Add(AppMsgType.Registration,
                msg => msg.ReadMessage<RegistrationMessage>());
            _appMsgHandlers.Add(AppMsgType.RegistrationAnswer,
                msg => msg.ReadMessage<RegistrationAnswerMessage>());
            _appMsgHandlers.Add(AppMsgType.UpdatePlayerDataAnswer,
                msg => msg.ReadMessage<AppUpdatePlayerDataAnswerMessage>());
            _appMsgHandlers.Add(AppMsgType.LoadDMarketDataRequest,
                msg => msg.ReadMessage<AppLoadDMarketDataMessage>());
            _appMsgHandlers.Add(AppMsgType.LoadDMarketDataAnswer,
                msg => msg.ReadMessage<AppLoadDMarketAnswerMessage>());
            _appMsgHandlers.Add(AppMsgType.ItemChangingRequest,
                msg => msg.ReadMessage<AppChangingItemsMessage>());
            _appMsgHandlers.Add(AppMsgType.ItemChangingAnswer,
                msg => msg.ReadMessage<AppChangingItemsAnswerMessage>());
            _appMsgHandlers.Add(AppMsgType.GetGameTokenRequest,
              msg => msg.ReadMessage<AppGetGameTokenMessage>());
            _appMsgHandlers.Add(AppMsgType.GetGameTokenAnswer,
                msg => msg.ReadMessage<AppGetGameTokenAnswerMessage>());
            _appMsgHandlers.Add(AppMsgType.UpdateMarketToken,
                msg => msg.ReadMessage<AppUpdateMarketTokenMessage>());
            _appMsgHandlers.Add(AppMsgType.DMarketTransactionCompleted,
                msg => msg.ReadMessage<AppDMarketTransactionMessage>());


#if UNITY_EDITOR
            if (Enum.GetValues(typeof(AppMsgType)).Length != _appMsgHandlers.Count + 1)
            {
                throw new System.Exception("You forget add monkey code for app " +
                    "messages!!!");
            }
            if (Enum.GetValues(typeof(GameMsgType)).Length != _gameMsgHandlers.Count + 1)
            {
                throw new System.Exception("You forget add monkey code for game " +
                    "messages!!!");
            }
#endif
        }

        public static GameMessageBase ReadGameMessage(NetworkMessage msg)
        {
            var type = (GameMsgType)msg.msgType;
            if (_gameMsgHandlers.ContainsKey(type))
            {
                var result = _gameMsgHandlers[type](msg);
                result.ClientId = msg.conn.connectionId;
                return result;
            }
            else
            {
                DevLogger.Error("Have no recive method for " + type, TankGameLogChannel.Network);
                return null;
            }
        }

        public static AppMessageBase ReadAppMessage(NetworkMessage msg)
        {
            var type = (AppMsgType)msg.msgType;
            if (_appMsgHandlers.ContainsKey(type))
            {
                var result = _appMsgHandlers[type](msg);
                result.ConnectionId = msg.conn.connectionId;
                return result;
            }
            else
            {
                DevLogger.Error("Have no recive method for " + type, TankGameLogChannel.Network);
                return null;
            }
        }

        public static List<short> GetAllShortCodesForAppMessages()
        {
            return GetAllNumbersFromRange(
                (short)AppMsgType.Login, (short)AppMsgType.Highest);
        }

        public static List<short> GetAllShortCodesForGameMessages()
        {
            return GetAllNumbersFromRange((short)GameMsgType.UnitMoved,
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