using DMarketSDK.Market.Items;

namespace DMarketSDK.Market.Forms
{
    public class MySellOffersForm : TableWithFiltersFormBase<ItemsFormModel>
    {
        public override ItemModelType ItemType
        {
            get
            {
                return ItemModelType.MySellOffers;
            }
        }
    }
}