using System;
using DMarketSDK.IntegrationAPI.Internal;

//Refresh basic access token
namespace DMarketSDK.IntegrationAPI.Request.Auth
{
    public class BasicRefreshTokenRequest : BaseRequest<BasicRefreshTokenRequest, BasicRefreshTokenRequest.Response, BasicRefreshTokenRequest.RequestParams>
    {
        private const string Path = "/auth/basic/refresh-token";
		

        public BasicRefreshTokenRequest(string basicRefreshToken)
        {
			if (string.IsNullOrEmpty(basicRefreshToken)) throw new ArgumentNullException("basicRefreshToken");
            Params = new RequestParams
            {

			};
			WithBasicRefreshToken(basicRefreshToken);
        }

        public class RequestParams
        {

		}

        public class Response
		{		
			public long expiresAt;
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