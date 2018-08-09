using SHLibrary.StateMachine;

namespace TankGame.GameServer.States
{
    public class ServerCloseState : StateBase<ServerSceneController>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            Controller.Stop();
        }
    }
}
