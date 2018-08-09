using System;
using DMarketSDK.IntegrationAPI.Internal;

//Sell offers statuses
namespace DMarketSDK.IntegrationAPI.Request.MarketIntegration
{
    public class GetSellOfferStatusesRequest : BaseRequest<GetSellOfferStatusesRequest, GetSellOfferStatusesRequest.Response, GetSellOfferStatusesRequest.RequestParams>
    {
        private const string Path = "/market-integration/dmarket/sell-offer/statuses";
		

        public GetSellOfferStatusesRequest(string dmarketToken)
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
			public string[] statuses;
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