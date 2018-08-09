using DMarketSDK.Market.Items;
using System;

namespace DMarketSDK.Market.Forms
{
    public interface IFormWithItems
    {
        event Action<ItemActionType, MarketItemModel> ItemClicked;
    }
}