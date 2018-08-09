using DMarketSDK.IntegrationAPI.Request.MarketIntegration;
using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;
using System.Collections.Generic;

namespace DMarketSDK.Market.Commands.API
{
    public class LoadBuyItemsCommand : ApiCommandBase, ILoadMarketItemsCommand, ILoadPriceRangeCommand
    {
        private readonly ShowingItemsInfo _loadParameters;

        public PriceRangeCommandResult PriceRangeResult { get; private set; }
        public LoadMarketItemsCommandResult CommandResult { get; private set; }

        public LoadBuyItemsCommand(ShowingItemsInfo loadParameters)
        {
            _loadParameters = loadParameters;
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
                query = _loadParameters.SearchPattern,
                categories = _loadParameters.GetCategoriesBody(),
            };
            MarketApi.GetAggregatedSellOffers(requestArgs, OnSuccessLoadRangeCallback, OnError);
        }

        private void OnSuccessLoadRangeCallback(GetGameAggregatedClassesRequest.Response result, GetGameAggregatedClassesRequest.RequestParams request)
        {
            PriceRangeResult = new PriceRangeCommandResult(result.minPrice, result.maxPrice);
            LoadItems();
        }

        private void LoadItems()
        {
            var requestArgs = new GetGameAggregatedClassesRequest.RequestParams
            {
                limit = _loadParameters.Limit,
                offset = _loadParameters.Offset,
                categories = _loadParameters.GetCategoriesBody(),
                orderBy = _loadParameters.OrderBy,
                orderDir = _loadParameters.GetDirByBody(),
                query = _loadParameters.SearchPattern,
                priceFrom = _loadParameters.MinPriceRange,
                priceTo = _loadParameters.MaxPriceRange
            };
            MarketApi.GetAggregatedSellOffers(requestArgs, OnSuccessLoadItemsCallback, OnError);
        }

        private void OnSuccessLoadItemsCallback(GetGameAggregatedClassesRequest.Response response, GetGameAggregatedClassesRequest.RequestParams request)
        {
            var items = new List<MarketItemModel>();

            foreach (var itemInfo in response.Items)
            {
                var itemModel = new MarketItemModel
                {
                    SellOfferId = itemInfo.cheapestOfferId,
                    Tittle = itemInfo.title,
                    ImageUrl = itemInfo.imageUrl,
                    Updated = IntToDateTime(itemInfo.lastUpdate),
                    Price = itemInfo.cheapestPrice,
                    OffersCount = itemInfo.offersCount,
                    ClassId = itemInfo.classId,
                    Description = itemInfo.description
                };

                items.Add(itemModel);
            }

            CommandResult = new LoadMarketItemsCommandResult(items, response.total);
            Terminate(true);
        }
    }
}

