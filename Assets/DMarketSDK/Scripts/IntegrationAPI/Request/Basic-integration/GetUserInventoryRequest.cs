using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//Returns user assets which in market at the moment
namespace DMarketSDK.IntegrationAPI.Request.BasicIntegration
{
    public class GetUserInventoryRequest : BaseRequest<GetUserInventoryRequest, GetUserInventoryRequest.Response, GetUserInventoryRequest.RequestParams>
    {
        private const string Path = "/basic-integration/dmarket/user/inventory";
		
        public GetUserInventoryRequest(string gameToken, string dmarketToken)
        {
			if (string.IsNullOrEmpty(gameToken)) throw new ArgumentNullException("gameToken");
			if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");

            Params = new RequestParams
            {
			};
			WithGameToken(gameToken);
			WithDMarketToken(dmarketToken);
        }

        public class RequestParams
        {
		}

        public class Response
        {
			public class MarketAsset
			{
				public string assetId;
				public string classId;
				public string imageUrl;
				public string title;
			}
			public List<MarketAsset> Items;
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
    }
}