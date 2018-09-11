using TankGame.Network.Messages;
using TankGame.UI.Forms;

namespace TankGame.Authorization.States
{
    public class AuthorizationLoginState : AuthorizationFormStateBase<LoginForm>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            FormView.SendButton.onClick.AddListener(OnSignInClicked);
            FormView.SignUpClicked += OnSignUpClicked;
            FormView.BackClicked += OnBackClick;
        }

        public override void Finish()
        {
            base.Finish();
            FormView.SendButton.onClick.RemoveListener(OnSignInClicked);
            FormView.SignUpClicked -= OnSignUpClicked;
            FormView.BackClicked -= OnBackClick;
        }

        private void OnSignInClicked()
        {
            FormView.ShowWaitingForm();

            var message = new LoginMessage
            {
                UserName = FormView.LoginField.text,
                Password = FormView.PasswordField.text
            };
            Client.Send(message);
        }

        protected override void OnLoginCallback(LoginAnswerMessage message)
        {
            base.OnLoginCallback(message);

            FormView.HideWaitingForm();

            if (message.Error != NetworkMessageErrorType.None)
            {
                FormView.ShowError(NetworkMessagesInfo.GetMessage(message.Error));
                return;
            }

            Model.UserName = message.PlayerInfo.AuthInfo.UserName;
            Model.Password = message.PlayerInfo.AuthInfo.Password;
            Model.IsLogged = true;
            Model.SetChanges();
            Controller.Login.SafeRaise(message.PlayerInfo);
            ApplyState<AuthorizationLoggedState>();
        }

        private void OnSignUpClicked()
        {
            ApplyState<AuthorizationRegistrationState>();
        }
    }
}