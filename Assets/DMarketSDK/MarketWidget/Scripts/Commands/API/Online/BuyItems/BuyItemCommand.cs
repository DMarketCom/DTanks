using DMarketSDK.Common.Sound;
using DMarketSDK.IntegrationAPI.Request.MarketIntegration;
using DMarketSDK.Market.Items;

namespace DMarketSDK.Market.Commands.API
{
    public class BuyItemCommand : ApiItemOperationCommandBase
    {
        public BuyItemCommand(MarketItemModel itemModel) : base(itemModel)
        {
        }

        public override void Start()
        {
            base.Start();
            MarketApi.BuyItemRequest(TargetItemModel.SellOfferId, OnSuccess, OnError);
        }

        private void OnSuccess(UserSellOfferBuyRequest.Response result, UserSellOfferBuyRequest.RequestParams request)
        {
            var message = "Item purchased! It will appear on your DMarket wallet shortly.";
            Controller.SoundManager.Play(MarketSoundType.SimpleMessage);
            Controller.PopUpController.ShowSimpleNotification(message);
            Terminate(true);
        }
    }
}