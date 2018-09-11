using Game.Units.Components;

namespace Game.Tank.States
{
    public class TankIdleState : TankStateBase
    {
        private IUnitInsideInputComponent Input { get { return Controller.Input; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);
            Input.Move += Controller.Move;
            Input.Fire += Controller.Fire;
        }

        public override void Finish()
        {
            base.Finish();
            Input.Move -= Controller.Move;
            Input.Fire -= Controller.Fire;
        }
    }
}