using System;
using TankGame.Network.Messages;

namespace TankGame.Network.Server
{
    public interface IGameServer : IServerConnectionObsarvable
    {
        event Action<GameMessageBase> GameMsgReceived;

        void SendToAll(GameMessageBase message);

        void SendToPlayer(GameMessageBase message, int connectionId);

        void SendToAllExcept(GameMessageBase message, int exceptConnectionId);
    }
}