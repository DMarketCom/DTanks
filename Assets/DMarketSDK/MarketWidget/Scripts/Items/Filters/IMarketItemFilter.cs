using System.Collections.Generic;

namespace DMarketSDK.Market.Items
{
    public interface IMarketItemFilter
    {
        List<MarketItemModel> GetItems(List<MarketItemModel> items);
    }
}