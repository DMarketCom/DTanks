using SHLibrary.StateMachine;

namespace Game.Tank.States
{
    public class TankStateBase : StateBase<TankController, TankView>
    {
        protected TankModel Model { get { return Controller.Model; } }
    }
}