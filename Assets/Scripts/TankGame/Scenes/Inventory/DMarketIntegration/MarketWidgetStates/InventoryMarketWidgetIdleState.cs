using DMarketSDK.Domain;
using DMarketSDK.Market;
using TankGame.Inventory.States;

namespace TankGame.Inventory.DMarketIntegration
{
    public sealed class InventoryMarketWidgetIdleState : InventoryStateBase
    {
        private IMarketWidget MarketWidget { get { return Controller.Widget as IMarketWidget; } }

        private bool _isNeedReloadMarketData;

        public override void Start(object[] args = null)
        {
            base.Start(args);

            MarketWidget.LogoutEvent += OnLogout;
            MarketWidget.LoginEvent += OnLogin;
            MarketWidget.ClosedEvent += OnMarketClosed;

            Controller.View.Hide();
            MarketWidget.Open(Controller.MarketIntegrationModel);
        }

        public override void Finish()
        {
            base.Finish();

            MarketWidget.LoginEvent -= OnLogin;
            MarketWidget.LogoutEvent -= OnLogout;
            MarketWidget.ClosedEvent -= OnMarketClosed;
        }

        private void OnLogin(LoginEventData data)
        {
            _isNeedReloadMarketData = true;
            Model.BasicAccessToken = data.MarketAccessToken;
            Model.SetChanges();
        }

        private void OnLogout()
        {
            _isNeedReloadMarketData = false;
            Model.BasicAccessToken = string.Empty;
            Model.SetChanges();
        }

        private void OnMarketClosed()
        {
            if (MarketWidget.IsLogged && _isNeedReloadMarketData)
            {
                ApplyState<InventoryLoadDMarketItemsState>();
                return;
            }

            ApplyState<InventoryIdleState>();
        }
    }
}