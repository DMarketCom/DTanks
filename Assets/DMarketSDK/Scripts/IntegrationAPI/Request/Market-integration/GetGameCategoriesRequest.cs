using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//Game categories tree
namespace DMarketSDK.IntegrationAPI.Request.MarketIntegration
{
    public class GetGameCategoriesRequest : BaseRequest<GetGameCategoriesRequest, GetGameCategoriesRequest.Response, GetGameCategoriesRequest.RequestParams>
    {
        private const string Path = "/market-integration/dmarket/game/categories";
		

        public GetGameCategoriesRequest(string dmarketToken)
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
			public class CategoriesAsset
			{
				public string title;
				public string value;
			}
			public List<CategoriesAsset> Categories;

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