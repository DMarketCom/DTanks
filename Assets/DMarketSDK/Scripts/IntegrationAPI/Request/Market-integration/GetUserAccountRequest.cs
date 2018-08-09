using System;
using DMarketSDK.IntegrationAPI.Internal;

//User account information
namespace DMarketSDK.IntegrationAPI.Request.MarketIntegration
{
    public class GetUserAccountRequest : BaseRequest<GetUserAccountRequest, GetUserAccountRequest.Response, GetUserAccountRequest.RequestParams>
    {
        private const string Path = "/market-integration/dmarket/user/account";
		

        public GetUserAccountRequest(string dmarketToken)
        {
			if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");
            Params = new RequestParams
            {

			};
			WithDMarketToken(dmarketToken);
        }

        public class RequestParams
        {

		}

        public class Response
		{		
			public bool agreementsInfoIsConfirmed;
			public long agreementsInfoUpdated;
			public string blockchainCredentialsAddress;
			public string email;
			public bool emailVerified;
			public string id;
			public string username;
		}

        protected override string GetBasePath()
        {
			return Path;
		}

        public override RequestMethod GetMethod()
        {
            return RequestMethod.Get;
        }
    }
}