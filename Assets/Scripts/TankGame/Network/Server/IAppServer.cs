using System;
using TankGame.Network.Messages;

namespace TankGame.Network.Server
{
    public interface IAppServer : IServerConnectionObsarvable
    {
        event Action<AppMessageBase> AppMsgReceived;

        void Send(AppMessageBase message);
    }
}
