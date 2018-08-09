using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//Move user assets from game to marketWidget
namespace DMarketSDK.IntegrationAPI.Request.BasicIntegration
{
    public class AsyncToMarketRequest : BaseRequest<AsyncToMarketRequest, AsyncMarketResponse, AsyncToMarketRequest.RequestParams>
    {
        private const string Path = "/basic-integration/dmarket/async/to-market";
		

        public AsyncToMarketRequest(string gameToken, string dmarketToken, AssetToMarketModel[] items)
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
			public AssetToMarketModel[] items;
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