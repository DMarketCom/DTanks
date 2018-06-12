using DMarketSDK.Widget;
using Shop.States;

namespace Shop.DMarketIntegration.States
{
    public class ShopOpenBasicWidgetState : ShopStateBase
    {
        protected IWidget Widget { get { return Controller.Widget; } }

        private bool _isNeedReloadMarketData;

        public override void Start(object[] args = null)
        {
            base.Start(args);
            if (!Widget.IsInitialize)
            {
                ApplyState<ShopBasicWidgetInitializeState>();
            }
            else
            {
                Widget.Open();
                Widget.LogoutEvent += OnLogout;
                Widget.LoginEvent += OnLogin;
                Widget.ClosedEvent += OnWidgetClosed;
            }
        }

        public override void Finish()
        {
            base.Finish();
            Widget.LoginEvent -= OnLogin;
            Widget.LogoutEvent -= OnLogout;
            Widget.ClosedEvent -= OnWidgetClosed;
        }

        private void OnLogin(LoginEventData data)
        {
            _isNeedReloadMarketData = true;
            Model.GameToken = data.MarketAccessToken;
            Model.SetChanges();
        }

        private void OnLogout()
        {
            _isNeedReloadMarketData = false;
            Model.GameToken = string.Empty;
            Model.SetChanges();
        }

        private void OnWidgetClosed()
        {
            if (Controller.Widget.IsLogged)
            {
                if (_isNeedReloadMarketData)
                {
                    ApplyState<ShopLoadBasicWidgetDataState>();
                }
                else
                {
                    ApplyState<ShopIdleState>();
                }
            }
            else
            {
                ApplyState<ShopIdleState>();
            }
        }
    }
}