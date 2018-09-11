using SHLibrary.StateMachine;
using TankGame.Network.Client;
using TankGame.Network.Messages;

namespace TankGame.Authorization.States
{
    public abstract class AuthorizationStateBase : StateBase<AuthorizationSceneController, AuthorizationView>
    {
        protected AuthorizationModel Model { get { return Controller.Model;  } }

        protected IAppClient Client { get { return Controller.Client; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);
            Client.AppMsgReceived += OnMessageReceived;
        }

        public override void Finish()
        {
            base.Finish();
            Client.AppMsgReceived -= OnMessageReceived;
        }

        private void OnMessageReceived(AppMessageBase message)
        {
            switch (message.Type)
            {
                case AppMsgType.LoginAnswer:
                    OnLoginCallback(message as LoginAnswerMessage);
                    break;
                case AppMsgType.RegistrationAnswer:
                    OnRegistrationCallback(message as RegistrationAnswerMessage);
                    break;
            }
        }

        protected override void OnBackClick()
        {
            base.OnBackClick();
            Controller.BackClicked.SafeRaise();
        }

        protected virtual void OnLoginCallback(LoginAnswerMessage message)
        {
        }

        protected virtual void OnRegistrationCallback(RegistrationAnswerMessage message)
        {
        }
    }
}