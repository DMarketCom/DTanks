namespace Game.Tank.States
{
    public class TankDiedState : TankStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            (Controller as ITank).Broke();
        }
    }
}