using Networking.Client;
using Networking.Msg;
using SHLibrary.StateMachine;

namespace TankGame.Authorization.States
{
    public abstract class AuthorizationStateBase : 
        StateBase<AuthorizationSceneController, AuthorizationView>
    {
        protected AuthorizationModel Model { get { return Controller.Model;  } }

        protected IAppClient Client { get { return Controller.Client; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);
            Client.AppMsgReceived += OnMessageRecived;
        }

        public override void Finish()
        {
            base.Finish();
            Client.AppMsgReceived -= OnMessageRecived;
        }

        private void OnMessageRecived(AppMessageBase message)
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
        protected virtual void OnLoginCallback(LoginAnswerMessage message)
        {
        }

        protected virtual void OnRegistrationCallback(RegistrationAnswerMessage message)
        {
        }
    }
}