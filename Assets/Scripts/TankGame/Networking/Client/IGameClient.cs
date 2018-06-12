using Networking.Msg;
using System;

namespace Networking.Client
{
    public interface IGameClient : IClientConnectionObservable
    {
        event Action<GameMessageBase> GameMsgReceived;

        void Send(GameMessageBase message);
    }
}