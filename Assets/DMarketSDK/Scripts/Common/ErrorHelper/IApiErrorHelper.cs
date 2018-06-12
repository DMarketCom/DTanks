using DMarketSDK.IntegrationAPI;

namespace DMarketSDK.Common.ErrorHelper
{
    public interface IApiErrorHelper
    {
        string GetErrorMessage(ErrorCode code);
    }
}
