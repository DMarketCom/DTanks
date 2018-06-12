using Networking.Msg;
using SHLibrary.Utils;
using TankGame.Forms;

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

            if (message.Error != NetMsgErrorType.None)
            {
                FormView.ShowError(NetMsgErrorMessages.GetMessage(message.Error));
            }
            else
            {
                Model.UserName = message.Data.Autorziation.UserName;
                Model.Password = message.Data.Autorziation.Password;
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