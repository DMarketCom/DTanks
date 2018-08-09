using System;
using DMarketSDK.IntegrationAPI.Internal;

//Logout user account
namespace DMarketSDK.IntegrationAPI.Request.Auth
{
    public class LogoutRequest : BaseRequest<LogoutRequest, LogoutRequest.Response, LogoutRequest.RequestParams>
    {
        private const string Path = "/auth/dmarket/logout";
		

        public LogoutRequest(string basicToken, string dmarketToken)
        {
			if (string.IsNullOrEmpty(basicToken)) throw new ArgumentNullException("basicToken");
			if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");
            Params = new RequestParams
            {

			};
			WithBasicToken(basicToken);
			WithDMarketToken(dmarketToken);
        }

        public class RequestParams
        {

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
    }
}