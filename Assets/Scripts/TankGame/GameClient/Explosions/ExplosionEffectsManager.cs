using SHLibrary;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Explosions
{
    public class ExplosionEffectsManager : UnityBehaviourBase, IExplosionEffectsManager
    {
        #region IExplosionEffectsManager implementation
        void IExplosionEffectsManager.Play(Vector3 pos, ExplosionEffectType effectType)
        {
            _effectCreators[effectType].PlayEffect(pos);
        }
        #endregion

        [SerializeField]
        private ExplosionEffect _gunShootPrefab;
        [SerializeField]
        private ExplosionEffect _bulletExplosionPrefab;
        [SerializeField]
        private ExplosionEffect _tankExplosionPrefab;
        [SerializeField]
        private ExplosionEffect _pickUpEffectPrefab;

        private Dictionary<ExplosionEffectType, ExplostionPool> _effectCreators;

        protected override void Awake()
        {
            base.Awake();

            _effectCreators = new Dictionary<ExplosionEffectType, ExplostionPool>();
            _effectCreators.Add(ExplosionEffectType.BulletExplosion,
                new ExplostionPool(() => CreateNewExplosion(_bulletExplosionPrefab)));
            _effectCreators.Add(ExplosionEffectType.GunShoot,
                new ExplostionPool(() => CreateNewExplosion(_gunShootPrefab)));
            _effectCreators.Add(ExplosionEffectType.TankExplosion,
                new ExplostionPool(() => CreateNewExplosion(_tankExplosionPrefab)));
            _effectCreators.Add(ExplosionEffectType.ItemPickUp,
             new ExplostionPool(() => CreateNewExplosion(_pickUpEffectPrefab)));
        }

        private ExplosionEffect CreateNewExplosion(ExplosionEffect effect)
        {
            return GameObject.Instantiate(effect, transform);
        }
    }
}