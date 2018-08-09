using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

//Create sell offer
namespace DMarketSDK.IntegrationAPI.Request.MarketIntegration
{
    public class UserSellOfferRequest : BaseRequest<UserSellOfferRequest, UserSellOfferRequest.Response, UserSellOfferRequest.RequestParams>
    {
        private const string Path = "/market-integration/dmarket/user/sell-offer";
		

        public UserSellOfferRequest(string assetId, long price, string dmarketToken)
        {
			if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");
            Params = new RequestParams
            {
				assetId = assetId,
				price = price
			};
			WithDMarketToken(dmarketToken);
        }

        public class RequestParams
        {
			public string assetId;
			public long price;
		}

        public class Response
        {
            public string offerId;
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
				{"assetId", Params.assetId},
				{"price", Params.price}
			};
		}
		
    }
}