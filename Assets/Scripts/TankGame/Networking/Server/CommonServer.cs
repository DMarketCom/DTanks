using UnityEngine.Networking;
using System;
using SHLibrary.Logging;
using SHLibrary.Utils;
using Networking.Msg;
using System.Collections.Generic;

namespace Networking.Server
{
    public class CommonServer : IServerLifecycleManager, IServerConnectionObsarvable,
        IAppServer, IGameServer
    {
#region IServerLifecycleManager implementation
        public bool Start(int port)
        {
            try
            {
#if APPTYPE_WEBSERVER
                NetworkServer.useWebSockets = true;
#endif
                NetworkServer.Listen(port);
                NetworkServer.maxDelay = 0;

                var networkConfig = new ConnectionConfig();
                networkConfig.DisconnectTimeout = 100000;

                RegisterServerHandlers();
            }
            catch( Exception e) 
            {
                DevLogger.Error("Cannot run server " + e.Message, TankGameLogChannel.Network);
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
        public event Action<int> Disconected;
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

        public void SendToAllExept(GameMessageBase message, int exceptConnectionId)
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
            NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconect);

            RegisterAllMessageHandles(NetworkingUtil.GetAllShortCodesForAppMessages(),
                 OnAppMessageRecive);
            RegisterAllMessageHandles(NetworkingUtil.GetAllShortCodesForGameMessages(),
                OnGameMessageRecive);
        }

        private void RegisterAllMessageHandles(List<short> types,
            Action<NetworkMessage> hendler)
        {
            types.ForEach(item =>
            NetworkServer.RegisterHandler(item, arg => hendler(arg)));
        }

        private void OnConnect(NetworkMessage msg)
        {
            Connected.SafeRaise(msg.conn.connectionId);
        }

        private void OnDisconect(NetworkMessage msg)
        {
            Disconected.SafeRaise(msg.conn.connectionId);
        }

        private void OnGameMessageRecive(NetworkMessage msg)
        {
            GameMsgReceived.SafeRaise(NetworkingUtil.ReadGameMessage(msg));
        }

        private void OnAppMessageRecive(NetworkMessage msg)
        {
            AppMsgReceived.SafeRaise(NetworkingUtil.ReadAppMessage(msg));
        }
    }
}