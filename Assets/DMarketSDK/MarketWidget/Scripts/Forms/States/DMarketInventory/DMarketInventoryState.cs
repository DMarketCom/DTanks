using DMarketSDK.Market.Domain;

namespace DMarketSDK.Market.Forms
{
    public sealed class DMarketInventoryState : MarketItemsStateBase<DMarketInventoryForm, ItemsFormModel>
    {
        protected override MarketTabType MarketTab
        {
            get
            {
                return MarketTabType.DmarketInventory;
            }
        }

        public override void Start(object[] args = null)
        {
            base.Start(args);

            LoadFormItems();
        }
    }
}