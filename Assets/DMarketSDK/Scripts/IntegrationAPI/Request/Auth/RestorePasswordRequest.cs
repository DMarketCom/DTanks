using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//DMarket reset password
namespace DMarketSDK.IntegrationAPI.Request.Auth
{
    public class RestorePasswordRequest : BaseRequest<RestorePasswordRequest, RestorePasswordRequest.Response, RestorePasswordRequest.RequestParams>
    {
        private const string Path = "/auth/dmarket/restore-password";
		

        public RestorePasswordRequest(string basicToken, string email)
        {
			if (string.IsNullOrEmpty(basicToken)) throw new ArgumentNullException("basicToken");
            Params = new RequestParams
            {
				email = email
			};
			WithBasicToken(basicToken);
        }

        public class RequestParams
        {
			public string email;
		}

        public class Response
		{
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
				{"email", Params.email}
			};
		}
		
    }
}