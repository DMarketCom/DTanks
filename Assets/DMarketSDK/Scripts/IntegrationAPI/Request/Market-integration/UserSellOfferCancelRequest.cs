using System;
using DMarketSDK.IntegrationAPI.Internal;

//Cancel sell offer
namespace DMarketSDK.IntegrationAPI.Request.MarketIntegration
{
    public class UserSellOfferCancelRequest : BaseRequest<UserSellOfferCancelRequest, UserSellOfferCancelRequest.Response, UserSellOfferCancelRequest.RequestParams>
    {
        private const string Path = "/market-integration/dmarket/user/sell-offer/{sellOfferId}/cancel";
		private string _sellOfferId = default(string);

        public UserSellOfferCancelRequest(string sellOfferId, string dmarketToken)
        {
			if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");
			this._sellOfferId = sellOfferId;
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
		}

        protected override string GetBasePath()
        {

			return Path.Replace("{sellOfferId}", _sellOfferId);
		}

        public override RequestMethod GetMethod()
        {
            return RequestMethod.Post;
        }
    }
}