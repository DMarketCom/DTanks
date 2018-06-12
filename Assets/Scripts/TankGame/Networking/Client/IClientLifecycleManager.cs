namespace Networking.Client
{
    public interface IClientLifecycleManager
    {
        void Start(string serverIP, int port);

        void Shutdown();
    }
}