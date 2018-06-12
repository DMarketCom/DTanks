using Networking.Msg;
using System;

namespace Networking.Server
{
    public interface IAppServer : IServerConnectionObsarvable
    {
        event Action<AppMessageBase> AppMsgReceived;

        void Send(AppMessageBase message);
    }
}
