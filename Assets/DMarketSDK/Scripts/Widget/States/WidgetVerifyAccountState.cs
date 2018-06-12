using DMarketSDK.Widget.Forms;

namespace DMarketSDK.Widget.States
{
    public class WidgetVerifyAccountState : WidgetFormStateBase<WidgetVerifyAccountForm>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            FormView.BtnLogin.onClick.AddListener(OnLoginClicked);
        }

        public override void Finish()
        {
            base.Finish();
            FormView.BtnLogin.onClick.RemoveListener(OnLoginClicked);
        }

        private void OnLoginClicked()
        {
            ApplyState<WidgetLoginState>();
        }
    }
}