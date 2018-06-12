using System;
using Networking.Client;
using Networking.Msg;
using PlayerData;
using SHLibrary.Logging;
using SHLibrary.StateMachine;
using TankGame.Authorization.States;

namespace TankGame.Authorization
{
    public class AuthorizationSceneController : StateMachineBase<AuthorizationView>
    {
        public Action<PlayerInfo> Login;
        public Action LogOut;
        public Action PlayClicked;
        public Action ShopClicked;

        public AuthorizationModel Model { get; private set; }

        public IAppClient Client { get; private set; }

        public void Run(bool isLogged, string userName, string password,
            IAppClient client)
        {
            Model = new AuthorizationModel(isLogged, userName, password);
            View.ApplyModel(Model);
            Client = client;
            Client.AppMsgReceived += OnAppMsgReceived;
            if (Model.IsLogged)
            {
                ApplyState<AuthorizationLoggedState>();
            }
            else
            {
                ApplyState<AuthorizationLoginState>();
            }
        }

        public void Shutdown()
        {
            Client.AppMsgReceived -= OnAppMsgReceived;
            ApplyState<AuthorizationEmptyState>();
        }

        private void OnAppMsgReceived(AppServerAnswerMessageBase answer)
        {
            var logString = "Client get app answer type {0} with error {1}";
            DevLogger.Log(string.Format(logString,
                answer.Type, answer.Error), TankGameLogChannel.Network);
        }
    }
}