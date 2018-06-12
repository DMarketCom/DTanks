using Game.Units.Components;
using UnityEngine;

namespace Game.Tank.States
{
    public class TankIdleState : TankStateBase
    {
        private IUnitInsideInputComponent Input { get { return Controller.Input; } }

        private IWeaponInsideComponent Weapon { get { return Controller.Weapon; } }
        
        public override void Start(object[] args = null)
        {
            base.Start(args);
            Input.Move += Controller.Move;
            Input.Fire += OnNeedFire;
            ScheduledUpdate(2f, true);
        }

        public override void Finish()
        {
            base.Finish();
            Input.Move -= Controller.Move;
            Input.Fire -= OnNeedFire;
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();
            //TODO tmp fix physic
            View.Body.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        private void OnNeedFire(Vector2 direction, float force)
        {
            if (Weapon.ReadyForFire)
            {
                const float fireDistance = 30f;

                View.Body.Translate(0, 0, fireDistance);
                var targetVector = View.Body.position;
                View.Body.Translate(0, 0, -fireDistance);
                
                Weapon.MakeFire(targetVector, force);
            }
        }
    }
}