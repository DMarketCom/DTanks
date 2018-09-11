using TankGame.Network.Messages;
using TankGame.UI.Forms;

namespace TankGame.Authorization.States
{
    public class AuthorizationRegistrationState : AuthorizationFormStateBase<RegisterForm>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            FormView.SendButton.onClick.AddListener(OnSignUpClicked);
            FormView.GoToLoginClicked -= OnGoToLoginClicked;
        }

        public override void Finish()
        {
            base.Finish();
            FormView.SendButton.onClick.RemoveListener(OnSignUpClicked);
            FormView.GoToLoginClicked -= OnGoToLoginClicked;
        }

        private void OnSignUpClicked()
        {
            FormView.ShowWaitingForm();

            var message = new RegistrationMessage
            {
                UserName = FormView.LoginField.text,
                Password = FormView.PasswordField.text
            };
            Client.Send(message);
        }

        protected override void OnRegistrationCallback(RegistrationAnswerMessage message)
        {
            base.OnRegistrationCallback(message);
            FormView.HideWaitingForm();
            if (message.Error != NetworkMessageErrorType.None)
            {
                FormView.ShowError(NetworkMessagesInfo.GetMessage(message.Error));
            }
            else
            {
                Model.UserName = FormView.LoginField.text;
                Model.Password = FormView.PasswordField.text;
                Model.SetChanges();
                ApplyState<AuthorizationSuccessRegistrationState>();
            }
        }

        private void OnGoToLoginClicked()
        {
            ApplyState<AuthorizationLoginState>();
        }
    }
}