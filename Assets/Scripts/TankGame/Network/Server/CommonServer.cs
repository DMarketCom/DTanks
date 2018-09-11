using System;
using System.Collections.Generic;
using SHLibrary.Logging;
using TankGame.Network.Messages;
using UnityEngine;
using UnityEngine.Networking;

namespace TankGame.Network.Server
{
    public class CommonServer : IAppServer, IGameServer
    {
        #region IServerConnectionObsarvable implementation

        public event Action<int> Connected;
        public event Action<int> Disconnected;

        #endregion

        #region interface IAppServer implementation

        public event Action<AppMessageBase> AppMsgReceived;

        public void Send(AppMessageBase message)
        {
            NetworkServer.SendToClient(message.ConnectionId, (short) message.Type, message);
            #if NETWORK_DEBUG
            DevLogger.Log("SendAppMessage: " + message.Type + "\n" + Newtonsoft.Json.JsonConvert.SerializeObject(message, Newtonsoft.Json.Formatting.Indented));
            #endif
        }

        #endregion

        #region IGameServer iterface implementation

        public event Action<GameMessageBase> GameMsgReceived;

        public void SendToAll(GameMessageBase message)
        {
            #if NETWORK_DEBUG
            DevLogger.Log("SendToAll: " + message.Type + "\n" + Newtonsoft.Json.JsonConvert.SerializeObject(message, Newtonsoft.Json.Formatting.Indented));
            #endif

            NetworkServer.SendToAll((short) message.Type, message);
        }

        public void SendToPlayer(GameMessageBase message, int connectionId)
        {
            #if NETWORK_DEBUG
            DevLogger.Log("SendToPlayer: " + connectionId + " " + message.Type + "\n" + Newtonsoft.Json.JsonConvert.SerializeObject(message, Newtonsoft.Json.Formatting.Indented));
            #endif
            NetworkServer.SendToClient(connectionId, (short) message.Type, message);

        }

        public void SendToAllExcept(GameMessageBase message, int exceptConnectionId)
        {
            #if NETWORK_DEBUG
            DevLogger.Log("SendToAllExcept " + exceptConnectionId + " " + message.Type + "\n" + Newtonsoft.Json.JsonConvert.SerializeObject(message, Newtonsoft.Json.Formatting.Indented));
            #endif

            foreach (var connection in NetworkServer.connections)
            {
                if (connection != null && connection.connectionId != exceptConnectionId)
                {
                    NetworkServer.SendToClient(
                        connection.connectionId,
                        (short) message.Type,
                        message);
                }
            }
        }

        #endregion

        public bool Start(int port)
        {
            try
            {
                NetworkServer.useWebSockets = UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer;

                NetworkServer.Listen(port);
                NetworkServer.maxDelay = 0;

                RegisterServerHandlers();
            }
            catch (Exception e)
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
            GameMessageBase gameMessage = NetworkingUtil.ReadGameMessage(msg);
            #if NETWORK_DEBUG
            DevLogger.Log("OnGameMessageReceive received: " + gameMessage.Type + "\n" + Newtonsoft.Json.JsonConvert.SerializeObject(gameMessage, Newtonsoft.Json.Formatting.Indented));
            #endif

            GameMsgReceived.SafeRaise(gameMessage);
        }

        private void OnAppMessageReceive(NetworkMessage msg)
        {
            AppMessageBase appMessage = NetworkingUtil.ReadAppMessage(msg);
            #if NETWORK_DEBUG
            DevLogger.Log("OnAppMessageReceive received: " + appMessage.Type + "\n" + Newtonsoft.Json.JsonConvert.SerializeObject(appMessage, Newtonsoft.Json.Formatting.Indented));
            #endif

            AppMsgReceived.SafeRaise(appMessage);
        }
    }
}