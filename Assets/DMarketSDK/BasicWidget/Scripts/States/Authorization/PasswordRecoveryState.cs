using DMarketSDK.Forms;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.IntegrationAPI.Request.Auth;

namespace DMarketSDK.Basic.States
{
    public sealed class PasswordRecoveryState : BasicWidgetFormStateBase<PasswordRecoveryForm, RecoveryPasswordFormModel>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);

            View.ClearFields();
            View.HideError();

            View.SendButton.onClick.AddListener(OnSendClick);
            View.EmailField.onEndEdit.AddListener(OnAccountEmailEndEdit);
            View.CloseButton.onClick.AddListener(OnCloseClicked);
            View.BackButton.onClick.AddListener(OnCloseClicked);
        }

        public override void Finish()
        {
            base.Finish();

            View.SendButton.onClick.RemoveListener(OnSendClick);
            View.EmailField.onEndEdit.RemoveListener(OnAccountEmailEndEdit);
            View.CloseButton.onClick.RemoveListener(OnCloseClicked);
            View.BackButton.onClick.RemoveListener(OnCloseClicked);
        }

        protected override void ShowError(ErrorCode errorCode)
        {
            base.ShowError(errorCode);
            var errorMessage = Controller.GetErrorMessage(errorCode);
            View.ShowError(errorMessage);
        }

        private void OnAccountEmailEndEdit(string accountEmail)
        {
            FormModel.AccountEmail = accountEmail;
            FormModel.SetChanges();
        }

        private void OnCloseClicked()
        {
            ApplyState<BasicWidgetLoginFormState>();
        }

        private void OnErrorCallback(Error error)
        {
            View.SetWaitState(false);
            bool isConnectionLost = error.ErrorCode == ErrorCode.CannotResolveDestinationHost 
                                    || error.ErrorCode == ErrorCode.RequestTimeout;
            if (isConnectionLost)
            {
                ApplyState<ConnectionLostState>();
                return;
            }

            ShowError(error.ErrorCode);
        }

        private void OnSendClick()
        {
            View.SetWaitState(true);
            View.HideError();

            ApplySendingCommand();
        }

        private void OnCallback(RestorePasswordRequest.Response data, RestorePasswordRequest.RequestParams request)
        {
            View.SetWaitState(false);
            ApplyState<PasswordRecoveryDoneState>();
        }

        private void ApplySendingCommand()
        {
            View.SetWaitState(true);

            Controller.MarketApi.RestorePassword(WidgetModel.BasicAccessToken, FormModel.AccountEmail, OnCallback, OnErrorCallback);
        }
    }
}
