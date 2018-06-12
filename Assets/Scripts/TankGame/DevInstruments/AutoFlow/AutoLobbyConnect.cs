using Lobby;
using SHLibrary.Utils;

namespace DevInstruments.AutoFlow
{
    public class AutoLobbyConnect : AutoFlowBase<LobbySceneController>
    {
        protected override void ApplyFlowOperation()
        {
            var model = SceneController.Model;
            SceneController.LobbyWaitConnection.SafeRaise(model.ServerIP, model.Port);
        }
    }
}