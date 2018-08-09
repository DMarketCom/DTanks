using System;
using System.Collections.Generic;
using SHLibrary.Logging;
using TankGame.Network.Messages;
using UnityEngine.Networking;

namespace TankGame.Network.Client
{
    public class CommonClient : IAppClient, IGameClient
    {
        #region IClientConnectionObservable implementation

        public event Action Disconnected;
        public event Action Connected;

        #endregion

        #region IClientLifecycleManager implementation

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

        #endregion
        
        #region IAppClient implementation

        public event Action<AppServerAnswerMessageBase> AppMsgReceived;

        public void Send(AppMessageBase message)
        {
            message.ConnectionId = _currentClient.connection.connectionId;
            _currentClient.Send((short)message.Type, message);
        }

        #endregion

        #region IGameClient implementation

        public event Action<GameMessageBase> GameMsgReceived;

        public void Send(GameMessageBase message)
        {
            message.ClientId = _currentClient.connection.connectionId;
            _currentClient.Send((short)message.Type, message);
        }

        #endregion

        private NetworkClient _currentClient;

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
            DevLogger.Log(string.Concat("Connected: ", msg.conn.address));
            Connected.SafeRaise();
        }

        private void OnDisconnect(NetworkMessage msg)
        {
            Disconnected.SafeRaise();
        }

        private void OnGameMessageReceive(NetworkMessage msg)
        {
            GameMsgReceived.SafeRaise(NetworkingUtil.ReadGameMessage(msg));
        }

        private void OnAppMessageReceive(NetworkMessage msg)
        {
            AppMsgReceived.SafeRaise(NetworkingUtil.ReadAppMessage(msg) as AppServerAnswerMessageBase);
        }
    }
}