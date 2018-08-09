using System;
using System.Collections.Generic;
using SHLibrary.Logging;
using TankGame.Network.Messages;
using UnityEngine;
using UnityEngine.Networking;

namespace TankGame.Network.Server
{
    public class CommonServer : IServerLifecycleManager, IAppServer, IGameServer
    {
#region IServerLifecycleManager implementation
        public bool Start(int port)
        {
            try
            {
                NetworkServer.useWebSockets = UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer;

                NetworkServer.Listen(port);
                NetworkServer.maxDelay = 0;

                RegisterServerHandlers();
            }
            catch(Exception e) 
            {
                DevLogger.Error("Cannot run server " + e, DTanksLogChannel.Network);
                return false;
            }
            return true;
        }

        public void Shutdown()
        {
            NetworkServer.Shutdown();
        }
#endregion

#region IServerConnectionObsarvable implementation
        public event Action<int> Connected;
        public event Action<int> Disconnected;
#endregion

#region interface IAppServer implementation
        public event Action<AppMessageBase> AppMsgReceived;

        public void Send(AppMessageBase message)
        {
            NetworkServer.SendToClient(message.ConnectionId, (short)message.Type, message);
        }
#endregion

#region IGameServer iterface implementation
        public event Action<GameMessageBase> GameMsgReceived;

        public void SendToAll(GameMessageBase message)
        {
            NetworkServer.SendToAll((short)message.Type, message);
        }

        public void SendToPlayer(GameMessageBase message, int connectionId)
        {
            NetworkServer.SendToClient(connectionId, (short)message.Type, message);
        }

        public void SendToAllExcept(GameMessageBase message, int exceptConnectionId)
        {
            var connections = NetworkServer.connections;
            foreach (var connection in connections)
            {
                if (connection != null && connection.connectionId != exceptConnectionId)
                {
                    NetworkServer.SendToClient(
                        connection.connectionId,
                        (short)message.Type,
                        message);
                }
            }
        }
#endregion

        private void RegisterServerHandlers()
        {
            NetworkServer.RegisterHandler(MsgType.Connect, OnConnect);
            NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconnect);

            RegisterAllMessageHandles(NetworkingUtil.GetAllShortCodesForAppMessages(),
                 OnAppMessageReceive);
            RegisterAllMessageHandles(NetworkingUtil.GetAllShortCodesForGameMessages(),
                OnGameMessageReceive);
        }

        private void RegisterAllMessageHandles(List<short> types, Action<NetworkMessage> handler)
        {
            types.ForEach(item =>
            NetworkServer.RegisterHandler(item, arg => handler(arg)));
        }

        private void OnConnect(NetworkMessage msg)
        {
            Connected.SafeRaise(msg.conn.connectionId);
        }

        private void OnDisconnect(NetworkMessage msg)
        {
            Disconnected.SafeRaise(msg.conn.connectionId);
        }

        private void OnGameMessageReceive(NetworkMessage msg)
        {
            GameMsgReceived.SafeRaise(NetworkingUtil.ReadGameMessage(msg));
        }

        private void OnAppMessageReceive(NetworkMessage msg)
        {
            AppMsgReceived.SafeRaise(NetworkingUtil.ReadAppMessage(msg));
        }
    }
}