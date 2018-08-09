namespace DMarketSDK.IntegrationAPI.Settings
{
    public interface IServerApiSettings : IBaseApiSettings
    {
        string GameToken { get; }
    }
}