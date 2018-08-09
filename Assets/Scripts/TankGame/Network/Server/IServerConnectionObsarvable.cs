using System;

namespace TankGame.Network.Server
{
    public interface IServerConnectionObsarvable
    {
        event Action<int> Disconnected;
        event Action<int> Connected;
    }
}
