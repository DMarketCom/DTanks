using SHLibrary.StateMachine;

namespace TankGame.GameServer.Commands
{
    public abstract class ServerCommandBase : ScheduledCommandBase<ServerSceneController>
    {
        protected ServerModel Model { get { return Controller.Model; } }
    }
}