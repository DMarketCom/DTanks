using DMarketSDK.Market.Containers;
using DMarketSDK.Market.Items;

namespace DMarketSDK.Market.Forms
{
    public class BuyItemsForm : TableWithFiltersFormBase<BuyItemsFormModel>
    {
        public override ItemModelType ItemType
        {
            get
            {
                return ItemModelType.BuyItems;
            }
        }

        private PriceRangeFilterComponent _priceRangeFilter;

        private PriceRangeFilterComponent PriceRangeFilter
        {
            get { return _priceRangeFilter ? _priceRangeFilter : (_priceRangeFilter = Container.GetShowingComponent<PriceRangeFilterComponent>());}
        }

        protected override void OnModelChanged()
        {
            base.OnModelChanged();
            ApplyPriceRange(FormModel.MinPriceRange, FormModel.MaxPriceRange);
        }

        private void ApplyPriceRange(long minPrice, long maxPrice)
        {
            if (Container.IsInitialize)
            {
                PriceRangeFilter.ApplyPriceRange(minPrice, maxPrice);
            }
        }

        public void ClearPriceRange()
        {
            PriceRangeFilter.ApplyPriceRange(0, 0);
        }
    }
}
