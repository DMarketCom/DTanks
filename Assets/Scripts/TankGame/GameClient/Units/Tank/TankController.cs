using System;
using DG.Tweening;
using Game.Tank.States;
using Game.Units.Components;
using SHLibrary.StateMachine;
using SHLibrary.Utils;
using UnityEngine;

namespace Game.Tank
{
    public class TankController : StateMachineBase<TankView>, ITank, INetworkTank
    {
        #region ITank implementation
        public event Action<ITank> Died;
        public event Action<ITank> Moved;
        
        public Vector3 Pos
        {
            get { return View.Body.position; }
            set { View.Body.position = value; }
        }

        public float Rotation
        {
            get { return View.Body.eulerAngles.y; }
            set
            {
                var currentAngles = View.Body.eulerAngles;
                currentAngles.y = value;
                View.Body.eulerAngles = currentAngles;
            }
        }

        bool ITank.IsAlive { get { return Model.Health > 0; } }

        void ITank.Respawn(Vector3 point)
        {
            View.Show();
            Model.Health = Model.MaxHealth;
            Model.SetChanges();
            Healt.Damaged += OnDamaged;
            ApplyState<TankRespawnState>(point);
        }

        void ITank.Broke()
        {
            View.Hide();
            Healt.Damaged -= OnDamaged;
        }

        public IHealtInsideComponent Healt { get; set; }
        public IUnitInsideInputComponent Input { get; set; }
        public IWeaponInsideComponent Weapon { get; set; }
        public GameUnitBase Unit { get; set; }
        #endregion

        #region INetworkTank implementation
        void INetworkTank.Move(Vector3 pos, float time)
        {
            DOTween.Kill(View.Body, false);
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

        private void OnDamaged(float damage)
        {
            Model.Health -= damage;
            if (Model.Health <= 0)
            {
                Model.Health = 0;
                Died.SafeRaise(this);
                ApplyState<TankDiedState>();
            }
            Model.SetChanges();
        }
    }
}