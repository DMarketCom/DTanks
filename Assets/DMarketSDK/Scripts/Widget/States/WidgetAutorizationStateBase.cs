using DMarketSDK.IntegrationAPI;
using DMarketSDK.Widget.Forms;

namespace DMarketSDK.Widget.States
{
    public abstract class WidgetAutorizationStateBase<T> : WidgetFormStateBase<T>
        where T : WidgetAutorizationFormBase
    {
        #region abstract members

        protected abstract void ApplySendingCommand();
        #endregion

        protected string Login { get { return FormView.LoginField.text; } }
        protected string Password { get { return FormView.PasswordField.text; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);

            FormView.SendButton.onClick.AddListener(OnSendClick);
        }

        public override void Finish()
        {
            base.Finish();
            FormView.SetWaitState(false);
            HideError();

            FormView.SendButton.onClick.RemoveListener(OnSendClick);
        }

        protected virtual bool CheckCorrectInputFormat()
        {
            var result = PasswordFormatCheck() && LoginFormatCheck();
            return result;
        }

        protected virtual void OnErrorCallback(Error error)
        {
            FormView.SetWaitState(false);
            Controller.OnErrorCallback(error);
            if (error.ErrorCode == ErrorCode.CannotResolveDestinationHost 
                || error.ErrorCode == ErrorCode.RequestTimeout)
            {
                ApplyState<WidgetConnectionLostState>();
                return;
            }
            ShowError(error.ErrorCode);
        }

        protected void ShowError(WidgetErrorType errorCode)
        {
            var errorMessage = WidgetErrorHelper.GetErrorMessage(errorCode);
            FormView.ShowError(errorMessage);
        }

        protected void ShowError(ErrorCode errorCode)
        {
            var errorMessage = ApiErrorHelper.GetErrorMessage(errorCode);
            FormView.ShowError(errorMessage);
        }

        private void HideError()
        {
            FormView.HideError();
        }
        
        private void OnSendClick()
        {
            if (CheckCorrectInputFormat())
            {
                FormView.SetWaitState();
                ApplySendingCommand();
            }
        }

        private bool PasswordFormatCheck()
        {
            if (string.IsNullOrEmpty(Password))
            {
                ShowError(WidgetErrorType.EmptyPassword);
                return false;
            }
            return true;
        }

        private bool LoginFormatCheck()
        {
            if (string.IsNullOrEmpty(Login))
            {
                ShowError(WidgetErrorType.EmptyLogin);
                return false;
            }
            return true;
        }
    }
}