using System;
using TankGame.Network.Messages;

namespace TankGame.Network.Client
{
    public interface IAppClient : IClientConnectionObservable
    {
        event Action<AppServerAnswerMessageBase> AppMsgReceived;

        void Send(AppMessageBase message);
    }
}
