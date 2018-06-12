using TankGame.Forms;

namespace TankGame.Authorization.States
{
    public class AuthorizationSuccessRegistrationState : AuthorizationFormStateBase<SuccessRegistrationForm>
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
            ApplyState<AuthorizationLoginState>();
        }
    }
}