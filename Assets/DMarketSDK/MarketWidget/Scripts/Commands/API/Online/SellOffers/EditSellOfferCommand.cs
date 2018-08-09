using DMarketSDK.IntegrationAPI.Request.MarketIntegration;

namespace DMarketSDK.Market.Commands.API
{
    public class EditSellOfferCommand : ApiCommandBase
    {
        private readonly string _assetId;
        private readonly long _amount;
        private readonly string _currency;

        public EditSellOfferCommand(string assetId, long amount, string currency)
        {
            _assetId = assetId;
            _amount = amount;
            _currency = currency;
        }

        public override void Start()
        {
            base.Start();
            MarketApi.EditSellOfferRequest(_assetId, _amount, _currency,
                OnSuccess, OnError);
        }

        private void OnSuccess(PutUserSellOfferRequest.Response result, PutUserSellOfferRequest.RequestParams request)
        {
            Terminate(true);
        }
    }
}