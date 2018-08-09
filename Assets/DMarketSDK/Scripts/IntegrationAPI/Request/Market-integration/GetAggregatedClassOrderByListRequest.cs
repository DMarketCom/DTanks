using System;
using DMarketSDK.IntegrationAPI.Internal;

//Aggregated class orderBy allowed values parameters
namespace DMarketSDK.IntegrationAPI.Request.MarketIntegration
{
    public class GetAggregatedClassOrderByListRequest : BaseRequest<GetAggregatedClassOrderByListRequest, GetAggregatedClassOrderByListRequest.Response, GetAggregatedClassOrderByListRequest.RequestParams>
    {
        private const string Path = "/market-integration/dmarket/aggregated-class/order-by-list";
		

        public GetAggregatedClassOrderByListRequest(string dmarketToken)
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