using System;
using System.Collections.Generic;
using SHLibrary.Logging;
using TankGame.Network.Messages;
using UnityEngine.Networking;

namespace TankGame.Network.Client
{
    public class CommonClient : IAppClient, IGameClient
    {
        public event Action Disconnected;
        public event Action Connected;
        
        private NetworkClient _currentClient;

        #region IAppClient implementation

        public event Action<AppServerAnswerMessageBase> AppMsgReceived;

        public void Send(AppMessageBase message)
        {
            message.ConnectionId = _currentClient.connection.connectionId;
            _currentClient.Send((short)message.Type, message);

            #if NETWORK_DEBUG
            DevLogger.Log("Send AppMessageBase: " + message.Type + " body: \n" + Newtonsoft.Json.JsonConvert.SerializeObject(message, Newtonsoft.Json.Formatting.Indented));
            #endif
        }

        #endregion

        #region IGameClient implementation

        public event Action<GameMessageBase> GameMsgReceived;

        public void Send(GameMessageBase message)
        {
            message.ClientId = _currentClient.connection.connectionId;
            _currentClient.Send((short)message.Type, message);

            #if NETWORK_DEBUG
            DevLogger.Log("Send GameMessageBase: " + message.Type + " body: \n" + Newtonsoft.Json.JsonConvert.SerializeObject(message, Newtonsoft.Json.Formatting.Indented));
            #endif
        }

        #endregion

                public void Start(string serverIP, int port)
        {
            _currentClient = new NetworkClient();
            RegisterHandles();
            try
            {
                _currentClient.Connect(serverIP, port);
            }
            catch (Exception e)
            {
                DevLogger.Error(e.Message);
            }
        }

        public void Shutdown()
        {
            _currentClient.Shutdown();
        }

        private void RegisterHandles()
        {
            _currentClient.RegisterHandler(MsgType.Connect, OnConnect);
            _currentClient.RegisterHandler(MsgType.Disconnect, OnDisconnect);

            RegisterAllMessageHandles(NetworkingUtil.GetAllShortCodesForAppMessages(),
                OnAppMessageReceive);
            RegisterAllMessageHandles(NetworkingUtil.GetAllShortCodesForGameMessages(),
                OnGameMessageReceive);
        }

        private void RegisterAllMessageHandles(List<short> types, Action<NetworkMessage> handler)
        {
            types.ForEach(item =>
            _currentClient.RegisterHandler(item, arg => handler(arg)));
        }

        private void OnConnect(NetworkMessage msg)
        {
            DevLogger.Log(string.Concat("Connected to IP: ", msg.conn.address));
            Connected.SafeRaise();
        }

        private void OnDisconnect(NetworkMessage msg)
        {
            Disconnected.SafeRaise();
        }

        private void OnGameMessageReceive(NetworkMessage msg)
        {
            GameMessageBase gameMessage = NetworkingUtil.ReadGameMessage(msg);
            #if NETWORK_DEBUG
            DevLogger.Log("NetworkMessage received: " + gameMessage.Type + "\n" + Newtonsoft.Json.JsonConvert.SerializeObject(gameMessage, Newtonsoft.Json.Formatting.Indented));
            #endif

            GameMsgReceived.SafeRaise(gameMessage);
        }

        private void OnAppMessageReceive(NetworkMessage msg)
        {
            AppMessageBase appMessage = NetworkingUtil.ReadAppMessage(msg);
            #if NETWORK_DEBUG
            DevLogger.Log("NetworkMessage received: " + appMessage.Type + "\n" + Newtonsoft.Json.JsonConvert.SerializeObject(appMessage, Newtonsoft.Json.Formatting.Indented));
            #endif

            AppMsgReceived.SafeRaise(appMessage as AppServerAnswerMessageBase);
        }
    }
}