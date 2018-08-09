using DMarketSDK.IntegrationAPI.Request.MarketIntegration;

namespace DMarketSDK.Market.Commands.API
{
    public class CreateSellOfferCommand : ApiCommandBase
    {
        private readonly string _assetId;
        private readonly long _amount;
        private readonly string _currency;

        public CreateSellOfferCommand(string assetId, long amount, string currency)
        {
            _assetId = assetId;
            _amount = amount;
            _currency = currency;
        }

        public override void Start()
        {
            base.Start();
            MarketApi.CreateSellOfferRequest(_assetId, _amount, _currency,
                OnSuccess, OnError);
        }

        private void OnSuccess(UserSellOfferRequest.Response result, UserSellOfferRequest.RequestParams request)
        {
            Terminate(true);
        }
    }
}