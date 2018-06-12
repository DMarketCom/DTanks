using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//Moves user asset from game to market
namespace DMarketSDK.IntegrationAPI.Request.BasicIntegration
{
    public class ToMarketRequest : BaseRequest<ToMarketRequest, ToMarketRequest.Response, ToMarketRequest.RequestParams>
    {
        private const string Path = "/basic-integration/dmarket/to-market";
		
        public ToMarketRequest(string gameToken, string dmarketToken, string assetId, string classId)
        {
			if (string.IsNullOrEmpty(gameToken)) throw new ArgumentNullException("gameToken");
			if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");

            Params = new RequestParams
            {
				assetId = assetId,
				classId = classId
			};
			WithGameToken(gameToken);
			WithDMarketToken(dmarketToken);
        }

        public class RequestParams
        {
			public string assetId;
			public string classId;
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
			return new Dictionary<string, object>
			{
				{"assetId", Params.assetId},
			{"classId", Params.classId}
			};
		}
    }
}