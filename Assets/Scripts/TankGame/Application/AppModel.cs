using SHLibrary.ObserverView;
using TankGame.Domain.PlayerData;

namespace TankGame.Application
{
    public enum AppType
    {
        Client = 0,
        Server = 1,
        All = 10
    }

    public class AppModel : ObservableBase
    {
        public PlayerInfo PlayerModel;
    
        public AppType AppType;

        public AppModel()
        {
            PlayerModel = new PlayerInfo();
        }
    }
}