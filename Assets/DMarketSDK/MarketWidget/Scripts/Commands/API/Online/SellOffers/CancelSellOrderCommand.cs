using DMarketSDK.IntegrationAPI.Request.MarketIntegration;
using DMarketSDK.Market.Items;

namespace DMarketSDK.Market.Commands.API
{
    public sealed class CancelSellOrderCommand : ApiItemOperationCommandBase
    {
        public CancelSellOrderCommand(MarketItemModel itemModel) : base(itemModel)
        {
        }

        public override void Start()
        {
            base.Start();      
            MarketApi.CancelSellOfferRequest(TargetItemModel.SellOfferId, OnSuccessCallback, OnError);
            TargetItemModel.IsLockFromServer = true;
            TargetItemModel.SetChanges();
        }

        protected override void Finish()
        {
            base.Finish();
            TargetItemModel.IsLockFromServer = false;
            TargetItemModel.SetChanges();
        }

        private void OnSuccessCallback(UserSellOfferCancelRequest.Response result, UserSellOfferCancelRequest.RequestParams request)
        {
            Terminate(true);
        }

    }
}