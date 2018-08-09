using DMarketSDK.Domain;
using DMarketSDK.Forms;

namespace DMarketSDK.Basic.States
{
    public sealed class PasswordRecoveryDoneState : BasicWidgetFormStateBase<PasswordRecoveryDoneForm, WidgetFormModel>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            View.BtnLogin.onClick.AddListener(OnLoginClicked);
        }

        public override void Finish()
        {
            base.Finish();
            View.BtnLogin.onClick.RemoveListener(OnLoginClicked);
        }

        private void OnLoginClicked()
        {
            ApplyState<BasicWidgetLoginFormState>();
        }
    }
}
