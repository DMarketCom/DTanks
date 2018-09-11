using System;
using UnityEngine;

namespace Game.Bullet
{
    public interface IBullet
    {
        event Action<IBullet, Collider> Hit;

        void Fire(Vector3 startPos, Vector3 endPos, float force);

        Vector3 Pos { get; }

        float Damage { get; }

        int UnitID { get; }
    }
}