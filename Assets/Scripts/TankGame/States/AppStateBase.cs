using SHLibrary.StateMachine;

namespace States
{
    public abstract class AppStateBase : StateBase<AppController>
    {
        protected AppModel Model { get { return Controller.Model; } }
    }
}