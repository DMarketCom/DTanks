using System;

namespace Networking.Server
{
    public interface IServerConnectionObsarvable
    {
        event Action<int> Disconected;
        event Action<int> Connected;
    }
}
