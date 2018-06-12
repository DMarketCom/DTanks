using System;
using UnityEngine;

namespace Game.Units.Components
{
    public interface IWeaponOutsideComponent
    {
        event Action<IWeaponOutsideComponent, Vector3, float> MakedFire;

        Vector3 GunPos { get; }

        int UnitId { get; }
    }
}
