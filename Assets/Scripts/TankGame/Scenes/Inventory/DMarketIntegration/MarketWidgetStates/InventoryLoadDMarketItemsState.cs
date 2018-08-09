using TankGame.Inventory.Domain;
using TankGame.Inventory.States;

namespace TankGame.Inventory.DMarketIntegration
{
    /// <summary>
    /// This state loads the player's items from DMarket inventory to display them in the game inventory.
    /// </summary>
    public class InventoryLoadDMarketItemsState : InventoryStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);

            var request = new DMarketLoadDataRequest
            {
                MarketToken = Model.BasicAccessToken,
                Callback = OnDMarketDataLoaded
            };
            Controller.LoadDMarketData(request);

            View.Show();
            View.WaitingForm.Show();
        }

        private void OnDMarketDataLoaded(DMarketDataLoadResponse response)
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
