using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//DMarket classes by game
namespace DMarketSDK.IntegrationAPI.Request.MarketIntegration
{
    public class GetGameAggregatedClassesRequest : BaseRequest<GetGameAggregatedClassesRequest, GetGameAggregatedClassesRequest.Response, GetGameAggregatedClassesRequest.RequestParams>
    {
        private const string Path = "/market-integration/dmarket/game/aggregated-classes";
		

        public GetGameAggregatedClassesRequest(string dmarketToken, RequestParams args)
        {
			if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");
            Params = args;
            WithDMarketToken(dmarketToken);
        }

        public class RequestParams
        {
			public int limit;
			public int offset;
			public string query;
			public string categories;
			public string orderBy;
			public string orderDir;
			public long priceFrom;
			public long priceTo;
		}

        public class Response
		{		
			public class ItemsAsset
			{
				public string cheapestOfferId;
				public Price cheapestPrice;
				public string classId;
				public string description;
				public string imageUrl;
				public long lastUpdate;
				public long offersCount;
				public string title;
			}
			public List<ItemsAsset> Items;

			public long maxPrice;
			public long minPrice;
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
			{"offset", Params.offset},
			{"query", Params.query},
			{"categories", Params.categories},
			{"orderBy", Params.orderBy},
			{"orderDir", Params.orderDir},
			{"priceFrom", Params.priceFrom},
			{"priceTo", Params.priceTo}		
};
		}
    }
}