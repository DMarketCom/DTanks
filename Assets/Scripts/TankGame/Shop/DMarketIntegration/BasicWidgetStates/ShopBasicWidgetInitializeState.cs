using System;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.Widget;
using Shop.Domain;
using Shop.States;
using TankGame.Shop;

namespace Shop.DMarketIntegration.States
{
    public class ShopBasicWidgetInitializeState : ShopStateBase
    {
        private IWidget Widget { get { return Controller.Widget; } }
        private ClientApi DMarketApi { get { return Controller.WidgetApi; } }
        private ShopView View { get { return Controller.View; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);
            Controller.GetGameToken(new DMarketGameTokenRequest(OnGetGameToken));
            View.WaitingForm.Show();
        }

        private void OnGetGameToken(DMarketGameTokenResponce response)
        {
            View.WaitingForm.Hide();

            View.MessageBoxForm.Closed += OnMessageBoxClosed;

            //TODO tmp dessision for internet losing
            if (response == null)
            {
                response = new DMarketGameTokenResponce {ErrorText = "Check your internet connection"};
            }


            if (String.IsNullOrEmpty(response.ErrorText))
            {
                Model.GameToken = response.GameToken;
                Model.GameRefreshToken = response.RefreshToken;
                Model.SetChanges();
                Widget.Init(Controller.WidgetApi, Model.GameToken, Model.GameRefreshToken);
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
            if (Widget.IsInitialize)
            {
                ApplyState<ShopOpenBasicWidgetState>();
            }
            else
            {
                ApplyState<ShopIdleState>();
            }
        }
    }
}
