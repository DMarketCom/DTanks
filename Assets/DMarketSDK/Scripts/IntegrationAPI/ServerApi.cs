using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;
using DMarketSDK.IntegrationAPI.Request;
using DMarketSDK.IntegrationAPI.Request.Auth;
using DMarketSDK.IntegrationAPI.Request.BasicIntegration;
using DMarketSDK.IntegrationAPI.Settings;

namespace DMarketSDK.IntegrationAPI
{
    public class ServerApi : BaseApi<IServerApiSettings>
    {
        private string GameToken
        {
            get { return ApiSettings.GameToken; }
        }

        public void AsyncToMarket(
            string marketAccessToken,
            AssetToMarketModel[] items,
            ResultCallback<AsyncMarketResponse, AsyncToMarketRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new AsyncToMarketRequest(GameToken, marketAccessToken, items)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }
                                                   
        public void AsyncFromMarket(
            string marketAccessToken,
            string[] assetIds,
            ResultCallback<AsyncMarketResponse, AsyncFromMarketRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            List<AssetFromMarketModel> items = new List<AssetFromMarketModel>();
            foreach (string asset in assetIds)
            {
                items.Add(new AssetFromMarketModel() { assetId = asset });
            }

            new AsyncFromMarketRequest(GameToken, marketAccessToken, items.ToArray())
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void CheckAsyncOperation(string marketAccessToken,
            string[] operationIds,
            ResultCallback<AsyncOperationRequest.Response, AsyncOperationRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new AsyncOperationRequest(GameToken, marketAccessToken, operationIds)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        [Obsolete("Need use ToMarketAsync")]
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

        [Obsolete("Need use FromMarketAsync")]
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