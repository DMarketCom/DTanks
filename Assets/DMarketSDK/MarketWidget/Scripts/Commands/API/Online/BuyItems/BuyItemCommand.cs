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
            var message = string.Format("Congratulations! Your {0} will appear in DMarket inventory as soon as transaction will be processed. It may take up to 15 minutes", TargetItemModel.Tittle);
            Controller.SoundManager.Play(MarketSoundType.SimpleMessage);
            Controller.PopUpController.ShowSimpleNotification(message);
            Terminate(true);
        }
    }
}