using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//Game authentication and getting basic token for it
namespace DMarketSDK.IntegrationAPI.Request.Auth
{
    public class BasicTokenRequest : BaseRequest<BasicTokenRequest, BasicTokenRequest.Response, BasicTokenRequest.RequestParams>
    {
        private const string Path = "/auth/basic/token";
		

        public BasicTokenRequest(string gameToken, string gameUserId)
        {
			if (string.IsNullOrEmpty(gameToken)) throw new ArgumentNullException("gameToken");
            Params = new RequestParams
            {
				gameUserId = gameUserId
			};
			WithGameToken(gameToken);
        }

        public class RequestParams
        {
			public string gameUserId;
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
				{"gameUserId", Params.gameUserId}
			};
		}
		
    }
}