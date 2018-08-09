using UnityEngine.UI;

namespace DMarketSDK.Forms
{
    public class UserAgreementsForm : ShowDocumentFormBase<UserAgreementFormModel>
    {
        public Toggle IAgreeToggle;
        public Button BtnLogin;
        public Button BtnRegistration;

        protected override void OnModelChanged()
        {
            base.OnModelChanged();

            IAgreeToggle.isOn = FormModel.IsUserAgree;
            BtnLogin.interactable = FormModel.IsUserAgree;
            BtnRegistration.interactable = FormModel.IsUserAgree;
        }
    }
}
