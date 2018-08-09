using System.Collections.Generic;
using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;
using SHLibrary.ObserverView;

namespace DMarketSDK.Market
{
    public sealed class MarketContainerModel : ObservableBase
    {
        public ShowingItemsInfo ShowingItemsInfo;

        public List<MarketItemModel> FilteredItems { private set; get; }

        public long TotalItemsCount { get; private set; }

        public MarketContainerModel()
        {
            FilteredItems = new List<MarketItemModel>();
            ShowingItemsInfo = new ShowingItemsInfo();
        }

        public void SetItems(List<MarketItemModel> items, long totalItems)
        {
            FilteredItems = items;
            TotalItemsCount = totalItems;
        }

        public void Clear()
        {
            FilteredItems.Clear();
            TotalItemsCount = 0;
        }
    }
}