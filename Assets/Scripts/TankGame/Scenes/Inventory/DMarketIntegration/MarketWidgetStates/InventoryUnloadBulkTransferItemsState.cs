using TankGame.Inventory.Domain;
using TankGame.Inventory.States;

namespace TankGame.Inventory.DMarketIntegration
{
    /// <summary>
    /// This state makes a bulk transfer of all items from the player's Game inventory into the DMarket inventory.
    /// This logic required for using DMarket plugin into Android and iOS platforms.
    /// </summary>
    public class InventoryUnloadBulkTransferItemsState : InventoryStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);

            View.Show();
            View.WaitingForm.Show();

            var request = new DMarketLoadDataRequest
            {
                MarketToken = Controller.Model.BasicAccessToken,
                Callback = OnUnloadBulkTransferFinished
            };

            Controller.UnloadGameInventory.SafeRaise(request);
        }

        private void OnUnloadBulkTransferFinished(DMarketDataLoadResponse loadResponse)
        {
            View.WaitingForm.Hide();

            if (!loadResponse.HaveError)
            {
                Controller.UpdateInventoryData(loadResponse.Inventory);
                ApplyState<InventoryIdleState>();
            }
            else
            {
                View.MessageBoxForm.Show("Error", Controller.Widget.GetErrorMessage(loadResponse.Error));
                View.MessageBoxForm.Closed += OnCloseFormClicked;
            }
        }

        private void OnCloseFormClicked()
        {
            View.MessageBoxForm.Closed -= OnCloseFormClicked;
            ApplyState<InventoryIdleState>();
        }
    }
}