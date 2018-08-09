namespace DMarketSDK.IntegrationAPI.Settings
{
    public interface IBaseApiSettings
    {
        string TargetEnvironmentUrl { get; }
        bool UseDebug { get; }
    }
}