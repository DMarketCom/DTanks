using UnityEngine;

namespace Game.Tank.States
{
    public class TankRespawnState : TankStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            View.Show();
            View.Body.position = (Vector3)args[0];
            ApplyState<TankIdleState>();
        }
    }
}