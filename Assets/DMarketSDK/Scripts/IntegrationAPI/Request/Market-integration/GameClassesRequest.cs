using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//Class details with bulk support
namespace DMarketSDK.IntegrationAPI.Request.MarketIntegration
{
    public class GameClassesRequest : BaseRequest<GameClassesRequest, GameClassesRequest.Response, GameClassesRequest.RequestParams>
    {
        private const string Path = "/market-integration/dmarket/game/classes";
		

        public GameClassesRequest(string dmarketToken, string[] ids)
        {
			if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");
            Params = new RequestParams
            {
				ids = ids
			};
			WithDMarketToken(dmarketToken);
        }

        public class RequestParams
        {
			public string[] ids;
		}

        public class Response
		{		
			public class ItemsAsset
			{
				public string classId;
				public string description;
				public string imageUrl;
				public string title;
			}
			public List<ItemsAsset> Items;

		}

        protected override string GetBasePath()
        {
			return Path;
		}

        public override RequestMethod GetMethod()
        {
            return RequestMethod.Post;
        }
		protected override Dictionary<string, object> GetBody()
		{
			return new Dictionary<string, object>(){
				{"ids", Params.ids}
			};
		}
		
    }
}