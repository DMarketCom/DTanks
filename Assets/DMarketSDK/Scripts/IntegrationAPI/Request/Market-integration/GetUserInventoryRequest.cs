using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//User assets which in marketWidget at the moment
namespace DMarketSDK.IntegrationAPI.Request.MarketIntegration
{
    public class GetUserInventoryRequest : BaseRequest<GetUserInventoryRequest, GetUserInventoryRequest.Response, GetUserInventoryRequest.RequestParams>
    {
        private const string Path = "/market-integration/dmarket/user/inventory";
		

        public GetUserInventoryRequest(string dmarketToken, int limit = default(int), int offset = default(int), string query = default(string))
        {
			if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");
            Params = new RequestParams
            {
				limit = limit,
				offset = offset,
				query = query
			};
			WithDMarketToken(dmarketToken);
        }

        public class RequestParams
        {
			public int limit;
			public int offset;
			public string query;
		}

        public class Response
		{		
			public class ItemsAsset
			{
				public string assetId;
				public string blockchainAssetId;
				public string classId;
				public string imageUrl;
				public string title;
			}
			public List<ItemsAsset> Items;

			public int total;
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
			{"query", Params.query}		
};
		}
    }
}