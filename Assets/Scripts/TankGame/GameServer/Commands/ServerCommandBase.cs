using SHLibrary.StateMachine;

namespace GameServer.Commands
{
    public class ServerCommandBase : ScheduledCommandBase<ServerSceneController>
    {
        protected ServerModel Model { get { return Controller.Model; } }
    }
}