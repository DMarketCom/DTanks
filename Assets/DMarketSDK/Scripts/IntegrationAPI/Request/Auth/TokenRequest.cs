using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//DMarket account authentication and link with game account
namespace DMarketSDK.IntegrationAPI.Request.Auth
{
    public class TokenRequest : BaseRequest<TokenRequest, TokenRequest.Response, TokenRequest.RequestParams>
    {
        private const string Path = "/auth/dmarket/token";
		

        public TokenRequest(string basicToken, string email, string password)
        {
			if (string.IsNullOrEmpty(basicToken)) throw new ArgumentNullException("basicToken");
            Params = new RequestParams
            {
				email = email,
				password = password
			};
			WithBasicToken(basicToken);
        }

        public class RequestParams
        {
			public string email;
			public string password;
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
		protected override Dictionary<string, object> GetBody()
		{
			return new Dictionary<string, object>(){
				{"email", Params.email},
				{"password", Params.password}
			};
		}
		
    }
}