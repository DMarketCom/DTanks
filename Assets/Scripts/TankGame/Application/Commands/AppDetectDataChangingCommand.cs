using SHLibrary.StateMachine;
using TankGame.Network.Client;
using TankGame.Network.Messages;

namespace TankGame.Application.Commands
{
    public class AppDetectDataChangingCommand : ScheduledCommandBase<AppController>
    {
        public override bool IsIndependentFromState
        {
            get
            {
                return true;
            }
        }

        private IAppClient Client { get { return Controller.Client; } }

        public override void Start()
        {
            base.Start();
            Client.AppMsgReceived += OnAppMessageReceived;
            Client.Disconnected += OnDisconnected;
        }

        private void OnDisconnected()
        {
            Terminate(true);
        }

        protected override void Finish()
        {
            base.Finish();
            Client.AppMsgReceived -= OnAppMessageReceived;
            Client.Disconnected -= OnDisconnected;
        }

        private void OnAppMessageReceived(AppMessageBase message)
        {
            switch (message.Type)
            {
                case AppMsgType.UpdatePlayerDataAnswer:
                    OnUpdatePlayerData(message as AppUpdatePlayerDataAnswerMessage);
                    break;
                case AppMsgType.Logout:
                    Terminate(true);
                    break;
            }
        }

        private void OnUpdatePlayerData(AppUpdatePlayerDataAnswerMessage message)
        {
            Controller.Model.PlayerModel = message.Data;
            Controller.Model.SetChanges();
        }
    }
}