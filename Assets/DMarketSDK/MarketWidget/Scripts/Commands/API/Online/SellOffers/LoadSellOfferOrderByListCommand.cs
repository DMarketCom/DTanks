using DMarketSDK.IntegrationAPI.Request.MarketIntegration;
using DMarketSDK.Market.Domain;

namespace DMarketSDK.Market.Commands.API
{
    public class LoadSellOfferOrderByListCommand : LoadFiltersCommandBase
    {
        public override void Start()
        {
            base.Start();

            MarketApi.GetSellOrderByList(OnSuccesCallback, OnError);
        }

        private void OnSuccesCallback(GetSellOfferOrderByListRequest.Response result, GetSellOfferOrderByListRequest.RequestParams request)
        {
            foreach (var item in result.items)
            {
                ResultCategories.Add(new FilterCategory(GetFilterTitle(item), item));
            }
            Terminate(true);
        }
    }
}