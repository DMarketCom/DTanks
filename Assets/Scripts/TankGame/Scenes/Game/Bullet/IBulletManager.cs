using Game.Units.Components;
using System;
using UnityEngine;

namespace Game.Bullet
{
    public interface IBulletManager
    {
        event Action<Collider, IBullet> Hit;
        event Action<IBullet> BulletStarted;

        void AddWeapon(IWeaponComponent component);
        void RemoveWeapon(IWeaponComponent component);
    }
}