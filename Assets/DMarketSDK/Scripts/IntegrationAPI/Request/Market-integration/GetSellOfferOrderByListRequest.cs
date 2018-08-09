using System;
using DMarketSDK.IntegrationAPI.Internal;

//Sell offer orderBy allowed values parameters
namespace DMarketSDK.IntegrationAPI.Request.MarketIntegration
{
    public class GetSellOfferOrderByListRequest : BaseRequest<GetSellOfferOrderByListRequest, GetSellOfferOrderByListRequest.Response, GetSellOfferOrderByListRequest.RequestParams>
    {
        private const string Path = "/market-integration/dmarket/sell-offer/order-by-list";
		

        public GetSellOfferOrderByListRequest(string dmarketToken)
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
			public string[] items;
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