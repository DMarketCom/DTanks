using DMarketSDK.Widget.Forms;

namespace DMarketSDK.Widget.States
{
    public class WidgetLoggedState : WidgetFormStateBase<WidgetLoggedForm>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            FormView.BtnLogout.onClick.AddListener(OnLogoutClick);
            FormView.TxtLogged.text = "Logged: " + Model.LoggedUsername;
        }

        public override void Finish()
        {
            base.Finish();
            FormView.BtnLogout.onClick.RemoveListener(OnLogoutClick);
        }

        private void OnLogoutClick()
        {
            ApplyState<WidgetLogoutState>();
        }
    }
}