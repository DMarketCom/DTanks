using System;
using Game.Units.Components;
using SHLibrary;
using UnityEngine;

namespace Game.Bullet
{
    public class TankBulletManager : UnityBehaviourBase, IBulletManager
    {
        #region IBulletManager implementation

        public event Action<Collider, IBullet> Hitted;

        public event Action<IBullet> BulletStarted;

        void IBulletManager.AddWeapon(IWeaponOutsideComponent component)
        {
            component.MakedFire += OnMakedFire;
        }

        void IBulletManager.RemoveWeapon(IWeaponOutsideComponent component)
        {
            component.MakedFire -= OnMakedFire;
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

        private void OnMakedFire(IWeaponOutsideComponent weapon, Vector3 target,
                                 float force)
        {
            var bullet = _bulletPool.GetBullet();
            bullet.Fire(weapon.GunPos, target, force);
            bullet.Hitted += OnBulletHitted;
            BulletStarted.SafeRaise(bullet);
        }

        private void OnBulletHitted(IBullet bullet, Collider coll)
        {
            if (coll != null)
            {
                Hitted.SafeRaise(coll, bullet);
            }
            bullet.Hitted -= OnBulletHitted;
            _bulletPool.ReturnBullet(bullet);
        }

        private TankBullet CreateNewBullet()
        {
            return GameObject.Instantiate(_bulletPrefab, transform);
        }
    }
}
