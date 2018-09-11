using DMarketSDK.Forms;

namespace DMarketSDK.Basic.States
{
    public sealed class UserAgreementsState : ShowDocumentStateBase<UserAgreementsForm, UserAgreementFormModel>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            
            View.IAgreeToggle.onValueChanged.AddListener(OnIAgreeToggle);
            View.BtnLogin.onClick.AddListener(OnLoginClicked);
            View.BtnRegistration.onClick.AddListener(OnRegistrationClicked);
            OnIAgreeToggle(false);
        }

        public override void Finish()
        {
            base.Finish();

            View.IAgreeToggle.onValueChanged.RemoveListener(OnIAgreeToggle);
            View.BtnLogin.onClick.RemoveListener(OnLoginClicked);
            View.BtnRegistration.onClick.RemoveListener(OnRegistrationClicked);
        }

        protected override void OnCloseClicked()
        {
            base.OnCloseClicked();

            Controller.Close();
        }

        private void OnIAgreeToggle(bool value)
        {
            FormModel.IsUserAgree = value;
            FormModel.SetChanges();
        }

        private void OnLoginClicked()
        {
            ApplyState<BasicWidgetLoginFormState>();
            SaveUserAgreement();
        }

        private void OnRegistrationClicked()
        {
            ApplyState<RegistrationStepOneState>();
            SaveUserAgreement();
        }

        private void SaveUserAgreement()
        {
            SavePrefsManager.SetBool(SavePrefsManager.DMARKET_USER_AGREEMENTS, true);
        }
    }
}