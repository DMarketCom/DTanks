using DMarketSDK.IntegrationAPI.Request.MarketIntegration;
using DMarketSDK.Market.Domain;

namespace DMarketSDK.Market.Commands.API
{
    public class LoadBuyOfferOrderByListCommand : LoadFiltersCommandBase
    {
        public override void Start()
        {
            base.Start();
            MarketApi.GetBuyOrderByList(OnSuccesCallback, OnError);
        }

        private void OnSuccesCallback(GetSellOfferOrderByListRequest.Response result, GetSellOfferOrderByListRequest.RequestParams request)
        {
            for (int i = 0; i < result.items.Length; i++)
            {
                var target = result.items[i];
                ResultCategories.Add(new FilterCategory(GetFilterTitle(target), target));
            }
            Terminate(true);
        }
    }
}