using System;
using Game.Units.Components;
using SHLibrary;
using UnityEngine;

namespace Game.Bullet
{
    public class TankBulletManager : UnityBehaviourBase, IBulletManager
    {
        #region IBulletManager implementation

        public event Action<Collider, IBullet> Hit;

        public event Action<IBullet> BulletStarted;

        void IBulletManager.AddWeapon(IWeaponComponent component)
        {
            component.Fire += OnTankFire;
        }

        void IBulletManager.RemoveWeapon(IWeaponComponent component)
        {
            component.Fire -= OnTankFire;
        }

        #endregion

        private BulletPool _bulletPool;
        [SerializeField]
        private TankBullet _bulletPrefab;

        protected override void Awake()
        {
            base.Awake();
            _bulletPool = new BulletPool(CreateNewBullet);
        }

        private void OnTankFire(Vector3 fireDirection, Vector3 target, float force)
        {
            var bullet = _bulletPool.GetBullet();
            bullet.Fire(fireDirection, target, force);
            bullet.Hit += OnBulletHit;
            BulletStarted.SafeRaise(bullet);
        }

        private void OnBulletHit(IBullet bullet, Collider coll)
        {
            if (coll != null)
            {
                Hit.SafeRaise(coll, bullet);
            }
            bullet.Hit -= OnBulletHit;
            _bulletPool.ReturnBullet(bullet);
        }

        private TankBullet CreateNewBullet()
        {
            return Instantiate(_bulletPrefab, transform);
        }
    }
}
