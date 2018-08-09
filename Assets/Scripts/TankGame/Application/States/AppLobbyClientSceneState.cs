using TankGame.Network.Client;

namespace TankGame.Application.States
{
    public class AppLobbyClientSceneState : AppLobbySceneBaseState
    {
        private CommonClient Client { get { return Controller.Client; } }
        
        private string MessageText { get { return (Args.Length > 0 ? (string)Args[0] : string.Empty); } }

        protected override void OnSceneStarted()
        {
            base.OnSceneStarted();
            Controller.Client = new CommonClient();
            Client.Connected += OnClientConnected;
            SceneController.LobbyWaitConnection += OnLobbyWaitConnection;
            SceneController.LobbyBrokeConnection += OnLobbyBreakConnection;
            SceneController.LobbyBackEvent += OnLobbyBackEvent;

            if (!string.IsNullOrEmpty(MessageText))
            {
                SceneController.ShowMessage("Args", MessageText);
            }
        }

        public override void Finish()
        {
            base.Finish();
            Client.Connected -= OnClientConnected;
            SceneController.LobbyWaitConnection -= OnLobbyWaitConnection;
            SceneController.LobbyBrokeConnection -= OnLobbyBreakConnection;
            SceneController.LobbyBackEvent -= OnLobbyBackEvent;
        }

        private void OnLobbyWaitConnection(string host, int port)
        {
            Client.Start(host, port);
        }

        private void OnLobbyBreakConnection()
        {
            Client.Shutdown();
        }

        private void OnLobbyBackEvent()
        {
            base.OnBackClick();
            ApplyState<AppSelectTypeSceneState>();
        }

        private void OnClientConnected()
        {
            ApplyState<AppAuthorizationSceneState>();
        }
    }
}
