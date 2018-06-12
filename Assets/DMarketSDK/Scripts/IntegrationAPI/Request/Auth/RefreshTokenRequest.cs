using System;
using DMarketSDK.IntegrationAPI.Internal;

//Refresh DMarket token
namespace DMarketSDK.IntegrationAPI.Request.Auth
{
    public class RefreshTokenRequest : BaseRequest<RefreshTokenRequest, RefreshTokenRequest.Response, RefreshTokenRequest.RequestParams>
    {
        private const string Path = "/auth/dmarket/refresh-token";
		
        public RefreshTokenRequest(string dmarketRefreshToken)
        {
			if (string.IsNullOrEmpty(dmarketRefreshToken)) throw new ArgumentNullException("dmarketRefreshToken");

            Params = new RequestParams
            {
			};
			WithDMarketRefreshToken(dmarketRefreshToken);
        }

        public class RequestParams
        {
		}

        public class Response
        {
			public int expiresAt;
			public string refreshToken;
			public string token;
			public string tokenType;
		}

        protected override string GetBasePath()
        {
			return Path;
		}

        public override RequestMethod GetMethod()
        {
            return RequestMethod.Post;
        }
    }
}