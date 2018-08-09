using DMarketSDK.IntegrationAPI.Request.MarketIntegration;
using DMarketSDK.Market.Domain;

namespace DMarketSDK.Market.Commands.API
{
    public sealed class LoadBuyItemsPriceRangeCommand : ApiCommandBase, ILoadPriceRangeCommand
    {
        private readonly ShowingItemsInfo _showingItemsInfo;

        public PriceRangeCommandResult PriceRangeResult { get; private set; }

        public LoadBuyItemsPriceRangeCommand(ShowingItemsInfo showingItemsInfo)
        {
            _showingItemsInfo = showingItemsInfo;
        }

        public override void Start()
        {
            base.Start();
            LoadPriceRange();
        }

        private void LoadPriceRange()
        {
            var requestArgs = new GetGameAggregatedClassesRequest.RequestParams
            {
                query = _showingItemsInfo.SearchPattern,
                categories = _showingItemsInfo.GetCategoriesBody(),
            };

            MarketApi.GetAggregatedSellOffers(requestArgs, OnSuccessLoadRangeCallback, OnError);
        }

        private void OnSuccessLoadRangeCallback(GetGameAggregatedClassesRequest.Response result, GetGameAggregatedClassesRequest.RequestParams request)
        {
            PriceRangeResult = new PriceRangeCommandResult(result.minPrice, result.maxPrice);
            Terminate(true);
        }
    }
}
