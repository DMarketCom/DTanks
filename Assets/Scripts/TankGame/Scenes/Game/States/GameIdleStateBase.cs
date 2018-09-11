using Game.Bullet;
using Game.Tank;
using System.Collections.Generic;
using Game.Units.Components;
using UnityEngine;

namespace Game.States
{
    public abstract class GameIdleStateBase : GameStateBase
    {
        protected ITank Player { get { return Controller.Player; } }
        protected List<ITank> Opponents { get { return Controller.Opponents; } }
        protected IBulletManager BulletManager { get { return Context.BulletManager; } }
        protected GameUnitCatalog UnitCatalog { get { return Context.UnitCatalog; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);

            Player.Died += OnPlayerDied;

            BulletManager.Hit += OnBulletHit;
        }

        public override void Finish()
        {
            base.Finish();
            Player.Died -= OnPlayerDied;

            BulletManager.Hit -= OnBulletHit;
        }

        private void OnBulletHit(Collider collider, IBullet bullet)
        {
            var unit = collider.gameObject.GetComponent<TankHealthComponent>();
            if (unit != null)
            {
                OnUnitDamaged(unit, bullet);
            }
        }

        private void OnUnitDamaged(IHealthComponent unit, IBullet bullet)
        {
            unit.TakeDamage(bullet.Damage, bullet.UnitID);
        }

        protected virtual void OnPlayerDied(ITank tank)
        {
        }
    }
}