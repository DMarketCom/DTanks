using SHLibrary.ObserverView;
using PlayerData;

public enum AppType
{
    Client,
    Server,
    All,
    WebServer
}

public class AppModel : ObservableBase
{
    public PlayerInfo PlayerModel;
    public string DMarketToken = string.Empty;
    
    public AppType AppType;

    public AppModel()
    {
        PlayerModel = new PlayerInfo();
    }
}