using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//Moves asset from market to game
namespace DMarketSDK.IntegrationAPI.Request.BasicIntegration
{
    public class FromMarketRequest : BaseRequest<FromMarketRequest, FromMarketRequest.Response, FromMarketRequest.RequestParams>
    {
        private const string Path = "/basic-integration/dmarket/from-marketWidget";
		

        public FromMarketRequest(string gameToken, string dmarketToken, string assetId)
        {
			if (string.IsNullOrEmpty(gameToken)) throw new ArgumentNullException("gameToken");
			if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");
            Params = new RequestParams
            {
				assetId = assetId
			};
			WithGameToken(gameToken);
			WithDMarketToken(dmarketToken);
        }

        public class RequestParams
        {
			public string assetId;
		}

        public class Response
		{
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
				{"assetId", Params.assetId}
			};
		}
		
    }
}