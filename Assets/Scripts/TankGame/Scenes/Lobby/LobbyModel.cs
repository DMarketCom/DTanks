using SHLibrary.ObserverView;
using TankGame.Application;

namespace Lobby
{
    public class LobbyModel : ObservableBase
    {
        public readonly AppType LobbyType;
        public string ServerIP = "localhost";
        public int Port = 24000;

        public LobbyModel(AppType appType)
        {
            LobbyType = appType;
        }
    }
}