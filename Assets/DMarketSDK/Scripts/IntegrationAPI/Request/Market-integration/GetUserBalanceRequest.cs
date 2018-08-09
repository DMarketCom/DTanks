using System;
using DMarketSDK.IntegrationAPI.Internal;

//User balance
namespace DMarketSDK.IntegrationAPI.Request.MarketIntegration
{
    public class GetUserBalanceRequest : BaseRequest<GetUserBalanceRequest, GetUserBalanceRequest.Response, GetUserBalanceRequest.RequestParams>
    {
        private const string Path = "/market-integration/dmarket/user/balance";
		

        public GetUserBalanceRequest(string dmarketToken)
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
			public long amount;
			public string currency;
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