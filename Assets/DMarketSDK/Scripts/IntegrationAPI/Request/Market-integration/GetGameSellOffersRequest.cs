using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//Get aggregated sell offers by game id
namespace DMarketSDK.IntegrationAPI.Request.MarketIntegration
{
    public class GetGameSellOffersRequest : BaseRequest<GetGameSellOffersRequest, GetGameSellOffersRequest.Response, GetGameSellOffersRequest.RequestParams>
    {
        private const string Path = "/market-integration/dmarket/game/sell-offers";
		

        public GetGameSellOffersRequest(string dmarketToken, int limit = default(int), int offset = default(int))
        {
			if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");
            Params = new RequestParams
            {
				limit = limit,
				offset = offset
			};
			WithDMarketToken(dmarketToken);
        }

        public class RequestParams
        {
			public int limit;
			public int offset;
		}

        public class Response
		{		
			public class ItemsAsset
			{
				public long created;
				public string description;
				public Price fee;
				public string imageUrl;
				public Price price;
				public string sellOfferId;
				public string title;
			}
			public List<ItemsAsset> Items;

			public long total;
		}

        protected override string GetBasePath()
        {
			return Path;
		}

        public override RequestMethod GetMethod()
        {
            return RequestMethod.Get;
        }
		protected override Dictionary<string, object> GetQuery()
		{
			return new Dictionary<string, object>(){
				{"limit", Params.limit},
			{"offset", Params.offset}		
};
		}
    }
}