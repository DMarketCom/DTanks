using TankGame.Inventory.DMarketIntegration;

namespace TankGame.Inventory.States
{
    public class InventoryInitState : InventoryStateBase
    {
        private bool IsNeedLoadMarketItems
        {
            get { return Controller.Widget.IsLogged; }
        }

        public override void Start(object[] args = null)
        {
            base.Start(args);

            Controller.View.Hide();
            if (!IsNeedLoadMarketItems)
            {
                ApplyState<InventoryIdleState>();
                return;
            }

            if (Controller.IsBasicIntegration)
            {
                ApplyState<InventoryLoadBulkTransferItemsState>();
            }
            else
            {
                ApplyState<InventoryLoadDMarketItemsState>();
            }
        }
    }
}