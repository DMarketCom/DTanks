using DMarketSDK.Market.Items;

namespace DMarketSDK.Market.Forms
{
    public class DMarketInventoryForm : InventoryFormBase
    {
        #region TableFormBase implementation

        public override ItemModelType ItemType { get { return ItemModelType.MarketInventory; } }

        #endregion
    }
}