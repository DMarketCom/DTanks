using DMarketSDK.Forms;
using UnityEngine;

namespace DMarketSDK.Market.Forms
{
    public sealed class UserAgreementsState : ShowDocumentStateBase<UserAgreementsForm, UserAgreementFormModel>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);

            MarketView.SetActiveTabsPanel(false);
            MarketView.Header.SetActiveHeaderPanel(false);
            
            View.IAgreeToggle.onValueChanged.AddListener(OnIAgreeToggle);
            View.BtnLogin.onClick.AddListener(OnLoginClicked);
            View.BtnRegistration.onClick.AddListener(OnRegistrationClicked);
            OnIAgreeToggle(false);
        }

        public override void Finish()
        {
            base.Finish();

            MarketView.Header.SetActiveHeaderPanel(true);
            MarketView.Header.SetActiveLoggedHeaderPanel(false);

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
            ApplyState<MarketLoginFormState>();
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