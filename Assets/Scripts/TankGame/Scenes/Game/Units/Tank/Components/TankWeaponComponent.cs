using UnityEngine;
using System;
using SHLibrary;

namespace Game.Units.Components
{
    public class TankWeaponComponent : UnityBehaviourBase, IWeaponInsideComponent,
        IWeaponOutsideComponent
    {
        private float _lastFireTime = 0;
        private float _reloadTime = 1f;

        [SerializeField]
        private Transform _gun;
        [SerializeField]
        private GameUnitBase _unit;

        #region IWeaponOutsideComponent implementation

        public event Action<IWeaponOutsideComponent, Vector3, float> MakedFire;

        Vector3 IWeaponOutsideComponent.GunPos { get { return _gun.position; } }

        int IWeaponOutsideComponent.UnitId { get { return _unit.UnitID; } }

        #endregion

        #region IWeaponInsideComponent implementation

        bool IWeaponInsideComponent.ReadyForFire
        {
            get
            {
                return Time.timeSinceLevelLoad > _lastFireTime + _reloadTime;
            }
        }

        void IWeaponInsideComponent.MakeFire(Vector3 target, float power)
        {
            _lastFireTime = Time.timeSinceLevelLoad;
            MakedFire.SafeRaise(this, target, power);
        }

        #endregion
    }
}