using System;
using DG.Tweening;
using Game.Tank.States;
using Game.Units.Components;
using SHLibrary.StateMachine;
using UnityEngine;

namespace Game.Tank
{
    public class TankController : StateMachineBase<TankView>, ITank, INetworkTank
    {
        #region ITank implementation

        public event Action<ITank> Died;
        public event Action<ITank> Moved;
        
        public Vector3 Position
        {
            get { return View.Body.position; }
            set { View.Body.position = value; }
        }

        public float Rotation
        {
            get { return View.Body.eulerAngles.y; }
            set
            {
                var currentRotation = View.Body.eulerAngles;
                currentRotation.y = value;
                View.Body.eulerAngles = currentRotation;
            }
        }

        bool ITank.IsAlive { get { return Model.Health > 0; } }

        void ITank.Respawn(Vector3 point)
        {
            View.Show();
            Model.Health = Model.MaxHealth;
            Model.SetChanges();
            Health.Damaged += OnDamaged;
            ApplyState<TankRespawnState>(point);
        }

        void ITank.Broke()
        {
            View.Hide();
            Health.Damaged -= OnDamaged;
        }

        public IHealthComponent Health { get; set; }
        public IUnitInsideInputComponent Input { get; set; }
        public IWeaponComponent Weapon { get; set; }
        public GameUnitBase Unit { get; set; }

        #endregion

        #region INetworkTank implementation

        void INetworkTank.Move(Vector3 pos, float time)
        {
            DOTween.Kill(View.Body);
            View.Body.DOMove(pos, time);
        }

        void INetworkTank.Broke()
        {
            OnDamaged(Model.Health + 1);
            View.Hide();
        }

        #endregion

        public readonly TankModel Model = new TankModel();

        public void Move(Vector2 direction)
        {
            var timeDelta = Time.deltaTime;
            View.Body.Rotate(Vector3.up, direction.x * Model.RotateSpeed * timeDelta);
            View.Body.Translate(0, 0, direction.y * Model.MoveSpeed * timeDelta);
            Moved.SafeRaise(this);
        }

        public void Fire(Vector2 direction, float force)
        {
            if (!Weapon.ReadyForFire)
            {
                return;
            }

            const float fireDistance = 30f;

            Vector3 bulletPosition = View.Body.position + View.Body.forward * fireDistance;

            Weapon.MakeFire(bulletPosition, force);
        }

        private void OnDamaged(float damagedHealth)
        {
            var currentHealth = Model.Health;
            float newHealth = Mathf.Clamp(currentHealth - damagedHealth, 0f, int.MaxValue);

            Model.Health = newHealth;
            Model.SetChanges();

            if (!(this as ITank).IsAlive)
            {
                Died.SafeRaise(this);
                ApplyState<TankDiedState>();
            }

        }
    }
}