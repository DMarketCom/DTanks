using DMarketSDK.Basic;
using TankGame.Inventory.Domain;
using TankGame.Inventory.States;

namespace TankGame.Inventory.DMarketIntegration
{
    public class InventoryMarketInitializeState : InventoryStateBase
    {
        private IWidgetCore Widget { get { return Controller.Widget; } }
    
        public override void Start(object[] args = null)
        {
            base.Start(args);
            Controller.GetGameToken(new DMarketGameTokenRequest(OnGetGameToken));
            View.WaitingForm.Show();
        }

        private void OnGetGameToken(DMarketGameTokenResponse response)
        {
            View.WaitingForm.Hide();

            View.MessageBoxForm.Closed += OnMessageBoxClosed;

            //TODO tmp dessision for internet losing
            if (response == null)
            {
                response = new DMarketGameTokenResponse {ErrorText = "Check your internet connection"};
            }

            if (string.IsNullOrEmpty(response.ErrorText))
            {
                Model.BasicAccessToken = response.BasicAccessToken;
                Model.BasicRefreshToken = response.BasicRefreshToken;
                Model.SetChanges();

                Widget.Init(Model.BasicAccessToken, Model.BasicRefreshToken, Controller.ClientApi);
                OnMessageBoxClosed();
            }
            else
            {
                View.MessageBoxForm.Show("Error", response.ErrorText);
            }
        }

        private void OnMessageBoxClosed()
        {
            View.MessageBoxForm.Closed -= OnMessageBoxClosed;
            if (Widget.IsInitialized)
            {
                if (Controller.IsBasicIntegration)
                {
                    ApplyState<InventoryBasicWidgetIdleState>();
                }
                else
                {
                    ApplyState<InventoryMarketWidgetIdleState>();
                }
            }
            else
            {
                ApplyState<InventoryIdleState>();
            }
        }
    }
}
