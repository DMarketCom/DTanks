using Shop.Domain;
using Shop.States;

namespace Shop.DMarketIntegration.States
{
    public class ShopLoadBasicWidgetDataState : ShopStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            var request = new DMarketLoadDataRequest
            {
                MarketToken = Model.GameToken,
                Callback = OnDMarketDataLoaded
            };
            Controller.LoadDMarketData(request);
            View.WaitingForm.Show();
        }

        private void OnDMarketDataLoaded(DMarketDataLoadResponce responce)
        {
            View.WaitingForm.Hide();
            if (string.IsNullOrEmpty(responce.ErrorText))
            {
                Controller.UpdateInventoryData(responce.Inventory);
                ApplyState<ShopIdleState>();
            }
            else
            {
                View.MessageBoxForm.Show("Error", responce.ErrorText);
                View.MessageBoxForm.Closed += OnCloseFormClicked;
            }
        }

        private void OnCloseFormClicked()
        {
            View.MessageBoxForm.Closed -= OnCloseFormClicked;
            ApplyState<ShopIdleState>();
        }
    }
}
