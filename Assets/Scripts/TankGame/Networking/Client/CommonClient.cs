using UnityEngine.Networking;
using System;
using SHLibrary.Utils;
using SHLibrary.Logging;
using Networking.Msg;
using System.Collections.Generic;

namespace Networking.Client
{
    public class CommonClient : IClientLifecycleManager, IClientConnectionObservable,
        IAppClient, IGameClient
    {
        #region IClientConnectionObservable implementation
        public event Action Disconected;
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
            _currentClient.RegisterHandler(MsgType.Disconnect, OnDisconect);

            RegisterAllMessageHandles(NetworkingUtil.GetAllShortCodesForAppMessages(),
                OnAppMessageRecive);
            RegisterAllMessageHandles(NetworkingUtil.GetAllShortCodesForGameMessages(),
                OnGameMessageRecive);
        }

        private void RegisterAllMessageHandles(List<short> types,
            Action<NetworkMessage> hendler)
        {
            types.ForEach(item =>
            _currentClient.RegisterHandler(item, arg => hendler(arg)));
        }

        private void OnConnect(NetworkMessage msg)
        {
            DevLogger.Log(string.Concat("Connected: ", msg.conn.address));
            Connected.SafeRaise();
        }

        private void OnDisconect(NetworkMessage msg)
        {
            Disconected.SafeRaise();
        }

        private void OnGameMessageRecive(NetworkMessage msg)
        {
            GameMsgReceived.SafeRaise(NetworkingUtil.ReadGameMessage(msg));
        }

        private void OnAppMessageRecive(NetworkMessage msg)
        {
            AppMsgReceived.SafeRaise(NetworkingUtil.ReadAppMessage(msg) as AppServerAnswerMessageBase);
        }
    }
}