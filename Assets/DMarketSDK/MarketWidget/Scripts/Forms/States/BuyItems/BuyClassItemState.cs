using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;

namespace DMarketSDK.Market.Forms
{
	public class BuyClassItemState : MarketItemsStateBase<BuyClassItemsForm, ItemsFormModel>
	{
		protected override MarketTabType MarketTab
		{
			get
			{
				return MarketTabType.BuyItems;
			}
		}

	    public override void Start(object[] args)
	    {
            base.Start(args);

	        FormModel.SelectedItem = (MarketItemModel) args[0];
            LoadFormItems();
	    }
	}
}