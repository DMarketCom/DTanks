using System;
using DMarketSDK.IntegrationAPI.Internal;

//Buy sell offer
namespace DMarketSDK.IntegrationAPI.Request.MarketIntegration
{
    public class UserSellOfferBuyRequest : BaseRequest<UserSellOfferBuyRequest, UserSellOfferBuyRequest.Response, UserSellOfferBuyRequest.RequestParams>
    {
        private const string Path = "/market-integration/dmarket/user/sell-offer/{sellOfferId}/buy";
		private string _sellOfferId = default(string);

        public UserSellOfferBuyRequest(string sellOfferId, string dmarketToken)
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