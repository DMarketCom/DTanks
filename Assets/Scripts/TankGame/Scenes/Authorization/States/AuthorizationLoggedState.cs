using TankGame.UI.Forms;

namespace TankGame.Authorization.States
{
    public class AuthorizationLoggedState : AuthorizationFormStateBase<LoggedForm>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            FormView.BtnLogout.onClick.AddListener(OnLogoutClick);
            FormView.BtnPlay.onClick.AddListener(OnPlayClick);
            FormView.BtnInventory.onClick.AddListener(OnInventoryClick);
            FormView.TxtLogged.text = string.Format("Logged as {0}", Model.UserName);
        }

        public override void Finish()
        {
            base.Finish();
            FormView.BtnLogout.onClick.RemoveListener(OnLogoutClick);
            FormView.BtnInventory.onClick.RemoveListener(OnInventoryClick);
            FormView.BtnPlay.onClick.RemoveListener(OnPlayClick);
        }

        private void OnLogoutClick()
        {
            ApplyState<AuthorizationLogoutState>();
        }

        private void OnPlayClick()
        {
            Controller.PlayClicked.SafeRaise();
        }

        private void OnInventoryClick()
        {
            Controller.InventoryClicked.SafeRaise();
        }
    }
}