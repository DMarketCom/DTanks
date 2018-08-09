using DMarketSDK.Basic;
using DMarketSDK.Domain;
using TankGame.Inventory.Domain;
using TankGame.Inventory.States;

namespace TankGame.Inventory.DMarketIntegration
{
    public sealed class InventoryBasicWidgetIdleState : InventoryStateBase
    {
        private IBasicWidget BasicWidget { get { return Controller.Widget as IBasicWidget; } }

        private bool _isNeedReloadMarketData;

        public override void Start(object[] args = null)
        {
            base.Start(args);

            BasicWidget.LoginEvent += OnLogin;
            BasicWidget.PreLogoutEvent += OnPreLogoutWidget;
            BasicWidget.LogoutEvent += OnLogout;
            BasicWidget.ClosedEvent += OnMarketWidgetClosed;

            Controller.View.Hide();
            BasicWidget.Open();
        }

        public override void Finish()
        {
            base.Finish();

            BasicWidget.LoginEvent -= OnLogin;
            BasicWidget.PreLogoutEvent -= OnPreLogoutWidget;
            BasicWidget.LogoutEvent -= OnLogout;
            BasicWidget.ClosedEvent -= OnMarketWidgetClosed;
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

        private void OnPreLogoutWidget()
        {
            UnloadInventoryBulkTransfer();
        }

        private void OnMarketWidgetClosed()
        {
            if (BasicWidget.IsLogged)
            {
                if (_isNeedReloadMarketData)
                {
                    ApplyState<InventoryLoadBulkTransferItemsState>();
                }
                else
                {
                    ApplyState<InventoryIdleState>();
                }
            }
            else
            {
                ApplyState<InventoryIdleState>();
            }
        }

        private void UnloadInventoryBulkTransfer()
        {
            var request = new DMarketLoadDataRequest
            {
                MarketToken = Controller.Model.BasicAccessToken,
                Callback = OnUnloadBulkTransferFinished
            };

            Controller.UnloadGameInventory.SafeRaise(request);
        }

        private void OnUnloadBulkTransferFinished(DMarketDataLoadResponse loadResponse)
        {
            BasicWidget.Logout();

            if (!loadResponse.HaveError)
            {
                Controller.UpdateInventoryData(loadResponse.Inventory);
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
        }
    }
}