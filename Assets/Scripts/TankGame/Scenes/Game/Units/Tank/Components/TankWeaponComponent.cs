using UnityEngine;
using System;
using SHLibrary;

namespace Game.Units.Components
{
    public sealed class TankWeaponComponent : UnityBehaviourBase, IWeaponComponent
    {
        private float _lastFireTime;
        private float _reloadTime = 1f;

        [SerializeField]
        private Transform _turretFireTransform;

        #region IWeaponComponent implementation

        public event Action<Vector3, Vector3, float> Fire;

        Vector3 IWeaponComponent.WeaponDirection { get { return _turretFireTransform.position; } }

        bool IWeaponComponent.ReadyForFire { get { return Time.timeSinceLevelLoad > _lastFireTime + _reloadTime; } }

        void IWeaponComponent.MakeFire(Vector3 target, float power)
        {
            _lastFireTime = Time.timeSinceLevelLoad;
            Vector3 direction = ((IWeaponComponent) this).WeaponDirection;
            Fire.SafeRaise(direction, target, power);
        }

        #endregion
    }
}