using DMarketSDK.Forms;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.IntegrationAPI.Request.Auth;

namespace DMarketSDK.Basic.States
{
    public sealed class BasicWidgetLoginFormState : BasicWidgetFormStateBase<WidgetLoginForm, WidgetLoginFormModel>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);

            View.CloseClicked += OnCloseClicked;
            View.LoginClicked += OnLoginClicked;
            View.LoginEndEdit += OnLoginEndEdit;
            View.PasswordEndEdit += OnPasswordEndEdit;
            View.SignUpClicked += OnSignUpClicked;
            View.SignUpStepTwoClicked += OnSignUpStepTwoClicked;
            View.ForgotPasswordClicked += OnForgotPasswordClicked;
        }

        public override void Finish()
        {
            base.Finish();

            View.CloseClicked -= OnCloseClicked;
            View.LoginClicked -= OnLoginClicked;
            View.LoginEndEdit -= OnLoginEndEdit;
            View.PasswordEndEdit -= OnPasswordEndEdit;
            View.SignUpClicked -= OnSignUpClicked;
            View.SignUpStepTwoClicked -= OnSignUpStepTwoClicked;
            View.ForgotPasswordClicked -= OnForgotPasswordClicked;
        }

        private void OnPasswordEndEdit(string userPassword)
        {
            FormModel.UserPassword = userPassword;
            FormModel.SetChanges();
        }

        private void OnLoginEndEdit(string userLogin)
        {
            FormModel.UserLogin = userLogin;
            FormModel.SetChanges();
        }

        private void OnLoginClicked()
        {
            GetMarketAccessToken();
        }

        private void OnSignUpClicked()
        {
            ApplyState<RegistrationStepOneState>();
        }

        private void OnSignUpStepTwoClicked()
        {
            ApplyState<RegistrationStepTwoState>();
        }

        private void OnForgotPasswordClicked()
        {
            ApplyState<PasswordRecoveryState>();
        }

        private void OnCloseClicked()
        {
            Controller.Close();
        }

        private void OnErrorCallback(Error error)
        {
            if (error.ErrorCode == ErrorCode.BasicAccessTokenExpired)
            {
                Controller.MarketApi.GetBasicRefreshToken(WidgetModel.BasicRefreshToken, OnRefreshBasicTokenCallback, OnErrorCallback);
            }
            else
            {
                View.SetWaitState(false);
                ShowError(error.ErrorCode);
            }
        }        

        private void OnRefreshBasicTokenCallback(BasicRefreshTokenRequest.Response result, BasicRefreshTokenRequest.RequestParams request)
        {
            WidgetModel.SetBasicTokens(result.token, result.refreshToken);
            WidgetModel.SetChanges();

            GetMarketAccessToken();
        }

        private void GetMarketAccessToken()
        {
            if (!CheckCorrectInputFormat())
            {
                return;
            }

            View.HideError();
            View.SetWaitState(true);
            Controller.MarketApi.GetMarketAccessToken(WidgetModel.BasicAccessToken, FormModel.UserLogin, FormModel.UserPassword,
                OnMarketAccessTokenCallback,
                OnErrorCallback);
        }

        private void OnMarketAccessTokenCallback(TokenRequest.Response data, TokenRequest.RequestParams request)
        {
            View.SetWaitState(false);
            View.ClearFields();

            Controller.Login(FormModel.UserLogin, data.token, data.refreshToken);

            ApplyState<BasicWidgetLoggedFormState>();
        }

        private bool CheckCorrectInputFormat()
        {
            var result = LoginFormatCheck(FormModel.UserLogin) && PasswordFormatCheck(FormModel.UserPassword);
            return result;
        }

        private bool PasswordFormatCheck(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                ShowError(ErrorCode.EmptyPassword);
                return false;
            }
            return true;
        }

        private bool LoginFormatCheck(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                ShowError(ErrorCode.EmptyLogin);
                return false;
            }
            return true;
        }
        
        protected override void ShowError(ErrorCode errorCode)
        {
            base.ShowError(errorCode);
            View.ShowError(Controller.GetErrorMessage(errorCode));
        }
    }
}