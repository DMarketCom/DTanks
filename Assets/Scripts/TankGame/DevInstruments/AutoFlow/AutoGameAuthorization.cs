using Networking.Client;
using Networking.Msg;
using SHLibrary.Utils;
using TankGame.Authorization;

namespace DevInstruments.AutoFlow
{
    public class AutoGameAuthorization : AutoFlowBase<AuthorizationSceneController>
    {
        private IAppClient Client { get { return SceneController.Client; } }

        private void SendRegistration()
        {
            var message = new RegistrationMessage
            {
                UserName = Settings.LoginGame,
                Password = Settings.PasswordGame
            };
            Client.AppMsgReceived += OnAppMesgReceived;
            Client.Send(message);
        }

        private void SendLogin()
        {
            var message = new LoginMessage
            {
                UserName = Settings.LoginGame,
                Password = Settings.PasswordGame
            };
            Client.AppMsgReceived += OnAppMesgReceived;
            Client.Send(message);
        }

        protected override void ApplyFlowOperation()
        {
            if (SceneController.Model.IsLogged)
            {
                return;
            }
            SendLogin();
        }

        private void OnAppMesgReceived(AppServerAnswerMessageBase message)
        {
            Client.AppMsgReceived -= OnAppMesgReceived;
            switch (message.Type)
            {
                case AppMsgType.LoginAnswer:
                    OnLoginAnswer(message as LoginAnswerMessage);
                    break;
                case AppMsgType.RegistrationAnswer:
                    OnRegistrationAnswer(message as RegistrationAnswerMessage);
                    break;
            }
        }

        private void OnLoginAnswer(LoginAnswerMessage message)
        {
            if (message.HasError)
            {
                if (message.Error == NetMsgErrorType.UserNameNotRegister)
                {
                    SendRegistration();
                }
            }
            else
            {
                SceneController.Login.SafeRaise(message.Data);
                SceneController.ShopClicked.SafeRaise();
            }
        }

        private void OnRegistrationAnswer(RegistrationAnswerMessage message)
        {
            if (!message.HasError)
            {
                SendLogin();
            }
        }
    }
}   