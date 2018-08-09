using System;

namespace TankGame.Network.Client
{
    public interface IClientConnectionObservable
    {
        event Action Disconnected;
        event Action Connected;
    }
}