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
            FormView.SignUpButton.onClick.AddListener(OnSignUpClicked);
        }

        public override void Finish()
        {
            base.Finish();
            FormView.SendButton.onClick.RemoveListener(OnSignInClicked);
            FormView.SignUpButton.onClick.RemoveListener(OnSignUpClicked);
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
            }
            else
            {
                Model.UserName = message.Data.AuthInfo.UserName;
                Model.Password = message.Data.AuthInfo.Password;
                Model.IsLogged = true;
                Model.SetChanges();
                Controller.Login.SafeRaise(message.Data);
                ApplyState<AuthorizationLoggedState>();
            }
        }

        private void OnSignUpClicked()
        {
            ApplyState<AuthorizationRegistrationState>();
        }
    }
}