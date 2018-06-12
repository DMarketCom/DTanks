using SHLibrary.StateMachine;
using TankGame.Lobby;

namespace Lobby.States
{
    public class LobbyStateBase : StateBase<LobbySceneController, LobbyView>
    {
        protected LobbyModel Model { get { return Controller.Model; } }
    }
}