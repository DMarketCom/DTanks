using DMarketSDK.Forms;
using DMarketSDK.IntegrationAPI;

namespace DMarketSDK.Market.Forms
{
    public sealed class RegistrationStepOneState : MarketFormStateBase<RegisterStepOneForm, RegistrationFormModel>
    {
        private string Email { get { return FormModel.RegistrationData.Email; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);

            View.OnEmailEntered += OnEmailEntered;
            View.OnTermsOfUseToggleChecked += OnTermsOfUseToggleChecked;
            View.OnNewsletterToggleChecked += OnNewsletterToggleChecked;
            View.NextClicked += OnNextClickClicked;
            View.TermOfUseClicked += OnTermOfUseClicked;
            View.PrivacyPolicyClicked += OnPrivacyPolicyClicked;
            View.BackClicked += OnBackButton;
        }

        public override void Finish()
        {
            base.Finish();

            HideError();

            View.OnEmailEntered -= OnEmailEntered;
            View.OnTermsOfUseToggleChecked -= OnTermsOfUseToggleChecked;
            View.OnNewsletterToggleChecked -= OnNewsletterToggleChecked;
            View.NextClicked -= OnNextClickClicked;
            View.TermOfUseClicked -= OnTermOfUseClicked;
            View.PrivacyPolicyClicked -= OnPrivacyPolicyClicked;
            View.BackClicked -= OnBackButton;
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

        private void OnNewsletterToggleChecked(bool value)
        {
            FormModel.RegistrationData.NewsletterIsEnabled = value;
            FormModel.SetChanges();
        }


        private void OnTermOfUseClicked()
        {
            OpenUrl(FormModel.TermsOfUseUrl);
        }

        private void OnPrivacyPolicyClicked()
        {
            OpenUrl(FormModel.PrivacyPolicyUrl);
        }

        private void OnBackButton()
        {
            ApplyState<MarketLoginFormState>();
        }

        protected override void ShowError(ErrorCode errorCode)
        {
            var errorMessage = Controller.GetErrorMessage(errorCode);
            View.ShowError(errorMessage);
        }

        private void HideError()
        {
            View.HideError();
        }

        private void OnNextClickClicked()
        {
            if (string.IsNullOrEmpty(Email))
            {
                ShowError(ErrorCode.EmptyEmail);
                return;
            }

            if (!IsEmailFormat(Email))
            {
                return;
            }

            ApplyState<RegistrationStepTwoState>(FormModel.RegistrationData);
        }
    }
}