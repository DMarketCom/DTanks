using DMarketSDK.Domain;
using DMarketSDK.Market.Items;

namespace DMarketSDK.Market.Forms
{
    public class ItemInfoFormModel : WidgetFormModel
    {
        public ItemActionType Type;
        public MarketItemModel CurrentItem { get; set; }
    }
}