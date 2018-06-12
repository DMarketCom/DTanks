using Networking.Msg;
using System;

namespace Networking.Server
{
    public interface IGameServer : IServerConnectionObsarvable
    {
        event Action<GameMessageBase> GameMsgReceived;

        void SendToAll(GameMessageBase message);

        void SendToPlayer(GameMessageBase message, int connectionId);

        void SendToAllExept(GameMessageBase message, int exceptConnectionId);
    }
}