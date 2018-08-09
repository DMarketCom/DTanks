namespace TankGame.Network.Server
{
    public interface IServerLifecycleManager
    {
        bool Start(int port);

        void Shutdown();
    }
}