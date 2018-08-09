using System.Collections.Generic;
using DMarketSDK.Market.Items;

namespace DMarketSDK.Market.Commands.API
{
    public sealed class LoadMarketItemsCommandResult
    {
        public readonly List<MarketItemModel> MarketItems;
        public readonly long TotalItemsCount;

        public LoadMarketItemsCommandResult(List<MarketItemModel> itemsList, long totalItemsCount)
        {
            MarketItems = itemsList;
            TotalItemsCount = totalItemsCount;
        }
    }
}