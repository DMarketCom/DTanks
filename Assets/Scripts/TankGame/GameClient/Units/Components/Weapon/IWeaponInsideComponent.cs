using UnityEngine;

namespace Game.Units.Components
{
    public interface IWeaponInsideComponent
    {
        bool ReadyForFire { get; }
        void MakeFire(Vector3 target, float power);
    }
}