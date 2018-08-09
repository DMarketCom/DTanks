using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//Move assets from marketWidget to game
namespace DMarketSDK.IntegrationAPI.Request.BasicIntegration
{
    public class AsyncFromMarketRequest : BaseRequest<AsyncFromMarketRequest, AsyncMarketResponse, AsyncFromMarketRequest.RequestParams>
    {
        private const string Path = "/basic-integration/dmarket/async/from-market";
		

        public AsyncFromMarketRequest(string gameToken, string dmarketToken, AssetFromMarketModel[] items)
        {
			if (string.IsNullOrEmpty(gameToken)) throw new ArgumentNullException("gameToken");
			if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");
            Params = new RequestParams
            {
				items = items
			};
			WithGameToken(gameToken);
			WithDMarketToken(dmarketToken);
        }

        public class RequestParams
        {
			public AssetFromMarketModel[] items;
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
				{"items", Params.items}
			};
		}
		
    }
}