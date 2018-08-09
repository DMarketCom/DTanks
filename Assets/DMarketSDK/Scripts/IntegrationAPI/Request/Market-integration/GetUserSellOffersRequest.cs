using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//User sell offers
namespace DMarketSDK.IntegrationAPI.Request.MarketIntegration
{
    public class GetUserSellOffersRequest : BaseRequest<GetUserSellOffersRequest, GetUserSellOffersRequest.Response, GetUserSellOffersRequest.RequestParams>
    {
        private const string Path = "/market-integration/dmarket/user/sell-offers";
		

        public GetUserSellOffersRequest(string status, string dmarketToken, int limit = default(int), int offset = default(int), string query = default(string), string orderBy = default(string), string orderDir = default(string))
        {
			if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");
            Params = new RequestParams
            {
				limit = limit,
				offset = offset,
				query = query,
				orderBy = orderBy,
				orderDir = orderDir,
				status = status
			};
			WithDMarketToken(dmarketToken);
        }

        public class RequestParams
        {
			public int limit;
			public int offset;
			public string query;
			public string orderBy;
			public string orderDir;
			public string status;
		}

        public class Response
		{		
			public class ItemsAsset
			{
				public long created;
				public Price fee;
				public string imageUrl;
				public Price price;
				public string sellOfferId;
				public string status;
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
			{"offset", Params.offset},
			{"query", Params.query},
			{"orderBy", Params.orderBy},
			{"orderDir", Params.orderDir},
			{"status", Params.status}		
};
		}
    }
}