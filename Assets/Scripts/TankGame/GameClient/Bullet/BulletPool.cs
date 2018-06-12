using SHLibrary.ObjectPool;
using System;

namespace Game.Bullet
{
    public class BulletPool : PoolBase<TankBullet>
    {
        private Func<TankBullet> _bulletCreator;

        public BulletPool(Func<TankBullet> bulletCreator)
        {
            _bulletCreator = bulletCreator;
        }

        protected override TankBullet CreateObject()
        {
            return _bulletCreator.Invoke();
        }

        public IBullet GetBullet()
        {
            return GetObject() as IBullet;
        }

        public void ReturnBullet(IBullet bullet)
        {
            AddObjects(bullet as TankBullet);
        }
    }
}
