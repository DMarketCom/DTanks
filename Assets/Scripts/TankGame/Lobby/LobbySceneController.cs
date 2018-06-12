using System;
using SHLibrary.StateMachine;
using Lobby.States;
using TankGame.Lobby;

namespace Lobby
{
    public class LobbySceneController : StateMachineBase<LobbyView>
    {
        public Action<string, int> LobbyWaitConnection;
        public Action LobbyBrokeConnection;
        public Action LobbyBackEvent;

        public LobbyModel Model { get; private set; }

        public void StartScene(AppType appType)
        {
            Model = new LobbyModel(appType);
            View.ApplyModel(Model);
            ApplyState<LobbySetParametrsState>();
        }

        public void ShowMessage(string title, string message)
        {
            View.MessageBoxForm.Show(title, message);
        }
    }
}