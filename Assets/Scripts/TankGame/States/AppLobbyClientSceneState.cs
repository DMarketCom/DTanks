using Networking.Client;

namespace States
{
    public class AppLobbyClientSceneState : AppLobbySceneBaseState
    {
        private CommonClient Client { get { return Controller.Client; } }

        protected override void OnSceneStarted()
        {
            base.OnSceneStarted();
            Controller.Client = new CommonClient();
            Client.Connected += OnClientConnected;
            SceneController.LobbyWaitConnection += OnLobbyWaitConnection;
            SceneController.LobbyBrokeConnection += OnLobbyBreakConnection;
            SceneController.LobbyBackEvent += OnLobbyBackEvent;

            if (Args.Length > 0)
            {
                SceneController.ShowMessage("Args", (string)Args[0]); // TODO: WTF Args. Need refactoring.
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
            ApplyState<AppAutorizationSceneState>();
        }
    }
}
