using DMarketSDK.IntegrationAPI.Internal;
using DMarketSDK.IntegrationAPI.Request.Auth;

namespace DMarketSDK.IntegrationAPI
{
    public class ClientApi : BaseApi
    {
        #region Autorization request
        public void GetMarketAccessToken(
            string basicAccessToken,
            string email,
            string password,
            ResultCallback<TokenRequest.Response, TokenRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new TokenRequest(basicAccessToken, email, password)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void RegisterMarketAccount(
            string basicAccessToken,
            string email,
            string password,
            string username = null,
            ResultCallback<RegisterRequest.Response, RegisterRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new RegisterRequest(basicAccessToken, email, password, username)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void Logout(
            string basicAccessToken,
            string marketAccessToken,
            ResultCallback<LogoutRequest.Response, LogoutRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new LogoutRequest(basicAccessToken, marketAccessToken)
               .WithCallback(callback)
               .WithErrorCallback(errorCallback)
               .Execute(ApiTransport);
        }

        public void GetBasicRefreshToken(
            string basicRefreshToken,
            ResultCallback<BasicRefreshTokenRequest.Response, BasicRefreshTokenRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new BasicRefreshTokenRequest(basicRefreshToken)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }

        public void RestorePassword(
            string basicAccessToken, 
            string email,
            ResultCallback<RestorePasswordRequest.Response, RestorePasswordRequest.RequestParams> callback = null,
            ErrorCallback errorCallback = null
        )
        {
            new RestorePasswordRequest(basicAccessToken, email)
                .WithCallback(callback)
                .WithErrorCallback(errorCallback)
                .Execute(ApiTransport);
        }
        #endregion
    }
}