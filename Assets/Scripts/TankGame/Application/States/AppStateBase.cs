using SHLibrary.StateMachine;

namespace TankGame.Application.States
{
    public abstract class AppStateBase : StateBase<AppController>
    {
        protected AppModel Model { get { return Controller.Model; } }
    }
}