using System;
using System.Collections.Generic;
using DMarketSDK.IntegrationAPI.Internal;

namespace DMarketSDK.IntegrationAPI.Request.MarketIntegration
{
    //Edit sell offer
    public class PutUserSellOfferRequest : BaseRequest<PutUserSellOfferRequest, PutUserSellOfferRequest.Response, PutUserSellOfferRequest.RequestParams>
    {
        private const string Path = "/market-integration/dmarket/user/sell-offer";


        public PutUserSellOfferRequest(string offerId, long price, string dmarketToken)
        {
            if (string.IsNullOrEmpty(dmarketToken)) throw new ArgumentNullException("dmarketToken");
            Params = new RequestParams
            {
                offerId = offerId,
                price = price
            };
            WithDMarketToken(dmarketToken);
        }

        public class RequestParams
        {
            public string offerId;
            public long price;
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
            return RequestMethod.Put;
        }
        protected override Dictionary<string, object> GetBody()
        {
            return new Dictionary<string, object>(){
                {"offerId", Params.offerId},
                {"price", Params.price}
            };
        }

    }
}