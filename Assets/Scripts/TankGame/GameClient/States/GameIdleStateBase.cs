using Game.Bullet;
using Game.Tank;
using System.Collections.Generic;
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

            BulletManager.Hitted += OnHitted;
        }

        public override void Finish()
        {
            base.Finish();
            Player.Died -= OnPlayerDied;

            BulletManager.Hitted -= OnHitted;
        }

        private void OnHitted(Collider collider, IBullet bullet)
        {
            var unit = collider.gameObject.GetComponent<GameUnitBase>();
            if (unit != null)
            {
                var unitType = UnitCatalog.GetUnitType(unit.UnitID);
                if (unitType == GameUnitType.Player)
                {
                    OnPlayerHitted(unit, bullet);
                }
                else if (unitType == GameUnitType.Oponent)
                {
                    OnOponentHitted(unit, bullet);
                }
            }
        }

        protected virtual void OnPlayerHitted(GameUnitBase unit, IBullet bullet)
        {
            unit.TakeDamage(bullet.Damage, bullet.UnitID);
        }

        protected virtual void OnOponentHitted(GameUnitBase unit, IBullet bullet)
        {
            unit.TakeDamage(bullet.Damage, bullet.UnitID);
        }

        protected virtual void OnPlayerDied(ITank tank)
        {
        }
    }
}