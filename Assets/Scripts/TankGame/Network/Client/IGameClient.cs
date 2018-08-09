using System;
using TankGame.Network.Messages;

namespace TankGame.Network.Client
{
    public interface IGameClient : IClientConnectionObservable
    {
        event Action<GameMessageBase> GameMsgReceived;

        void Send(GameMessageBase message);
    }
}