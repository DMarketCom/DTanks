using System;
using UnityEngine;

namespace Game.Units.Components
{
    public interface IWeaponComponent
    {
        event Action<Vector3, Vector3, float> Fire;

        Vector3 WeaponDirection { get; }

        bool ReadyForFire { get; }

        void MakeFire(Vector3 target, float power);
    }
}