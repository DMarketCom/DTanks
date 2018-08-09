using Game.Units.Components;
using System;
using UnityEngine;

namespace Game.Bullet
{
    public interface IBulletManager
    {
        event Action<Collider, IBullet> Hitted;
        event Action<IBullet> BulletStarted;

        void AddWeapon(IWeaponOutsideComponent component);
        void RemoveWeapon(IWeaponOutsideComponent component);
    }
}
