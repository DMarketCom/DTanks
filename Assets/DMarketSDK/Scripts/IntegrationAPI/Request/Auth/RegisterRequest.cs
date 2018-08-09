using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//DMarket account register and link with game account
namespace DMarketSDK.IntegrationAPI.Request.Auth
{
    public class RegisterRequest : BaseRequest<RegisterRequest, RegisterRequest.Response, RegisterRequest.RequestParams>
    {
        private const string Path = "/auth/dmarket/register";
		

        public RegisterRequest(string basicToken, string email, string password, string username)
        {
			if (string.IsNullOrEmpty(basicToken)) throw new ArgumentNullException("basicToken");
            Params = new RequestParams
            {
				email = email,
				password = password,
				username = username
			};
			WithBasicToken(basicToken);
        }

        public class RequestParams
        {
			public string email;
			public string password;
			public string username;
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
				{"password", Params.password},
				{"username", Params.username}
			};
		}
		
    }
}