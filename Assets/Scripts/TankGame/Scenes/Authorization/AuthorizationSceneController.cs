using System;
using SHLibrary.Logging;
using SHLibrary.StateMachine;
using TankGame.Authorization.States;
using TankGame.Domain.PlayerData;
using TankGame.Network.Client;
using TankGame.Network.Messages;

namespace TankGame.Authorization
{
    public class AuthorizationSceneController : StateMachineBase<AuthorizationView>
    {
        public Action BackClicked;

        public Action<PlayerInfo> Login;
        public Action LogOut;
        public Action PlayClicked;
        public Action InventoryClicked;

        public AuthorizationModel Model { get; private set; }

        public IAppClient Client { get; private set; }

        public void Run(PlayerAuthInfo playerAuthInfo, IAppClient client)
        {
            Model = new AuthorizationModel(playerAuthInfo.IsLogged, playerAuthInfo.UserName, playerAuthInfo.Password);
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
        }

        private void OnAppMsgReceived(AppServerAnswerMessageBase answer)
        {
            var logString = "Client get app answer type {0} with error {1}";
            DevLogger.Log(string.Format(logString,
                answer.Type, answer.Error), DTanksLogChannel.Network);
        }
    }
}