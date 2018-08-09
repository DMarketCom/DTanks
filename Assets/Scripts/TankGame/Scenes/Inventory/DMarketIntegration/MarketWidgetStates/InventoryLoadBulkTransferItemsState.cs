using TankGame.Inventory.Domain;
using TankGame.Inventory.States;

namespace TankGame.Inventory.DMarketIntegration
{
    /// <summary>
    /// This state makes a bulk transfer of all items from the player's DMarket inventory into the Game inventory.
    /// This logic required for using DMarket plugin into Android and iOS platforms.
    /// </summary>
    public class InventoryLoadBulkTransferItemsState : InventoryStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);

            var request = new DMarketLoadDataRequest
            {
                MarketToken = Model.BasicAccessToken,
                Callback = OnLoadBulkTransferFinished
            };
            Controller.LoadGameInventory.SafeRaise(request);

            View.Show();
            View.WaitingForm.Show();
        }

        private void OnLoadBulkTransferFinished(DMarketDataLoadResponse response)
        {
            View.WaitingForm.Hide();
            if (!response.HaveError)
            {
                Controller.UpdateInventoryData(response.Inventory);
                ApplyState<InventoryIdleState>();
            }
            else
            {
                View.MessageBoxForm.Show("Error", Controller.Widget.GetErrorMessage(response.Error));
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