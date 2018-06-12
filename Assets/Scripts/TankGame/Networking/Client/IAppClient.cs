using Networking.Msg;
using System;

namespace Networking.Client
{
    public interface IAppClient : IClientConnectionObservable
    {
        event Action<AppServerAnswerMessageBase> AppMsgReceived;

        void Send(AppMessageBase message);
    }
}
