using DMarketSDK.IntegrationAPI.Internal;
using DMarketSDK.IntegrationAPI.Request.Auth;
using DMarketSDK.IntegrationAPI.Request.BasicIntegration;

namespace DMarketSDK.IntegrationAPI
{
    public class ServerApi : BaseApi
    {
        public void ToMarket(
            string marketAccessToken,
            string assetId,
            string classId,
            ResultCallback<ToMarketRequest.Response, ToMarketRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new ToMarketRequest(GameToken, marketAccessToken, assetId, classId)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void FromMarket(
            string marketAccessToken,
            string assetId,
            ResultCallback<FromMarketRequest.Response, FromMarketRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new FromMarketRequest(GameToken, marketAccessToken, assetId)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void GetInMarketInventory(
            string marketAccessToken,
            ResultCallback<GetUserInventoryRequest.Response, GetUserInventoryRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new GetUserInventoryRequest(GameToken, marketAccessToken)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void GetBasicAccessToken(
            string gameUserId,
            ResultCallback<BasicTokenRequest.Response, BasicTokenRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new BasicTokenRequest(GameToken, gameUserId)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void GetMarketRefreshToken(
            string marketResreshToken,
            ResultCallback<RefreshTokenRequest.Response, RefreshTokenRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new RefreshTokenRequest(marketResreshToken)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }
    }
}