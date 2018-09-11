using DMarketSDK.Domain;
using DMarketSDK.Forms;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.IntegrationAPI.Request.Auth;

namespace DMarketSDK.Market.Forms
{
    public sealed class RegistrationStepTwoState : MarketFormStateBase<RegisterStepTwoForm, RegistrationFormModel>
    {
        private MarketRegistrationData RegistrationData { get { return FormModel.RegistrationData;} }

        public override void Start(object[] args = null)
        {
            base.Start(args);

            if (args.Length > 0)
            {
                FormModel.RegistrationData = (MarketRegistrationData)args[0];
                FormModel.SetChanges();
            }
            else
            {
                InitializeModel();
            }

            View.OnEmailEntered += OnEmailEntered;
            View.OnTermsOfUseToggleChecked += OnTermsOfUseToggleChecked;
            View.OnReEnterPasswordEntered += OnReEnterPasswordEntered;
            View.OnPasswordEntered += OnPasswordEntered;
            View.OnAccountNameEntered += OnAccountNameEntered;
            View.CreateAccountClicked += OnCreateAccountClicked;
            View.BackClicked += OnBackClicked;
            View.CloseClicked += OnCloseClicked;

            View.TermOfUseClicked += OnTermOfUseClicked;
            View.PrivacyPolicyClicked += OnPrivacyPolicyClicked;
            View.SignInClicked += OnSignInClicked;
        }

        public override void Finish()
        {
            base.Finish();

            View.HideError();

            View.OnEmailEntered -= OnEmailEntered;
            View.OnTermsOfUseToggleChecked -= OnTermsOfUseToggleChecked;
            View.OnReEnterPasswordEntered -= OnReEnterPasswordEntered;
            View.OnPasswordEntered -= OnPasswordEntered;
            View.OnAccountNameEntered -= OnAccountNameEntered;
            View.CreateAccountClicked -= OnCreateAccountClicked;
            View.BackClicked -= OnBackClicked;
            View.CloseClicked -= OnCloseClicked;

            View.TermOfUseClicked -= OnTermOfUseClicked;
            View.PrivacyPolicyClicked -= OnPrivacyPolicyClicked;
            View.SignInClicked -= OnSignInClicked;
        }

        private void InitializeModel()
        {
            FormModel.RegistrationData = new MarketRegistrationData();
            FormModel.SetChanges();
        }

        private void OnSignInClicked()
        {
            ApplyState<MarketLoginFormState>();
        }

        private void OnEmailEntered(string value)
        {
            FormModel.RegistrationData.Email = value;
            FormModel.SetChanges();
        }

        private void OnTermsOfUseToggleChecked(bool value)
        {
            FormModel.RegistrationData.TermsOfUseIsEnabled = value;
            FormModel.SetChanges();
        }

        private void OnReEnterPasswordEntered(string value)
        {
            FormModel.RegistrationData.ReEnterPassword = value;
            FormModel.SetChanges();
        }

        private void OnPasswordEntered(string value)
        {
            FormModel.RegistrationData.Password = value;
            FormModel.SetChanges();
        }

        private void OnAccountNameEntered(string value)
        {
            FormModel.RegistrationData.AccountName = value;
            FormModel.SetChanges();
        }

        private void OnBackClicked()
        {
            ApplyState<RegistrationStepOneState>();
        }

        private void OnCloseClicked()
        {
            ApplyState<MarketLoginFormState>();
        }

        private void OnTermOfUseClicked()
        {
            OpenUrl(FormModel.TermsOfUseUrl);
        }

        private void OnPrivacyPolicyClicked()
        {
            OpenUrl(FormModel.PrivacyPolicyUrl);
        }

        private void OnCreateAccountClicked()
        {
            if (!CheckCorrectInputFormat())
            {
                return;
            }

            View.SetWaitState(true);

            Controller.MarketApi.RegisterMarketAccount(WidgetModel.BasicAccessToken, 
                RegistrationData.Email, RegistrationData.Password, RegistrationData.AccountName, OnCreateAccountCallback, OnErrorCallback);
        }

        private bool CheckCorrectInputFormat()
        {
            return IsEmailFormat(RegistrationData.Email) && PasswordFormatCheck() && IsPasswordsEqual();
        }

        private bool IsPasswordsEqual()
        {
            if (RegistrationData.Password != RegistrationData.ReEnterPassword)
            {
                ShowError(ErrorCode.PasswordsAreNotEqual);
                return false;
            }
            return true;
        }

        private void OnCreateAccountCallback(RegisterRequest.Response data, RegisterRequest.RequestParams request)
        {
            View.SetWaitState(false);

            ApplyState<VerifyAccountState>();
        }

        private bool PasswordFormatCheck()
        {
            if (string.IsNullOrEmpty(RegistrationData.Password))
            {
                ShowError(ErrorCode.EmptyPassword);
                return false;
            }
            return true;
        }

        protected override void ShowError(ErrorCode errorCode)
        {
            View.ShowError(Controller.ErrorHelper.GetErrorMessage(errorCode));
        }

        private void OnErrorCallback(Error error)
        {
            View.SetWaitState(false);

            if (error.ErrorCode == ErrorCode.CannotResolveDestinationHost
                || error.ErrorCode == ErrorCode.RequestTimeout)
            {
                ApplyState<ConnectionLostState>();
                return;
            }
            ShowError(error.ErrorCode);
        }
    }
}