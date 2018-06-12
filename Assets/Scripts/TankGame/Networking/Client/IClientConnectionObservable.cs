using System;

namespace Networking.Client
{
    public interface IClientConnectionObservable
    {
        event Action Disconected;
        event Action Connected;
    }
}