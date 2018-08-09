using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//Returns user assets which in marketWidget at the moment
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
			public class ItemsAsset
			{
				public string assetId;
				public string classId;
			}
			public List<ItemsAsset> Items;

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