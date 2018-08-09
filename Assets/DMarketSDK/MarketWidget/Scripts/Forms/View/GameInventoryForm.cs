using DMarketSDK.Market.Items;

namespace DMarketSDK.Market.Forms
{
    public class GameInventoryForm : InventoryFormBase
    {
        #region TableFormBase implementation

        public override ItemModelType ItemType { get { return ItemModelType.GameInventory; } }

        #endregion
    }
}