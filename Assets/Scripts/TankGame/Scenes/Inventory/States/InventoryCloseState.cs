using SHLibrary.StateMachine;
using TankGame.Inventory.Domain;

namespace TankGame.Inventory.States
{
    public class InventoryCloseState : StateBase<InventorySceneController>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);

            if (Controller.IsBasicIntegration)
            {
                Controller.View.WaitingForm.Show();

                var request = new DMarketLoadDataRequest
                {
                    MarketToken = Controller.Model.BasicAccessToken,
                    Callback = OnUnloadBulkTransferInventory
                };

                Controller.UnloadGameInventory.SafeRaise(request);
            }
            else
            {
                Controller.ToPreviousScene.SafeRaise();
            }
        }

        private void OnUnloadBulkTransferInventory(DMarketDataLoadResponse dMarketDataLoadResponse)
        {
            Controller.View.WaitingForm.Hide();
            Controller.ToPreviousScene.SafeRaise();
        }
    }
}