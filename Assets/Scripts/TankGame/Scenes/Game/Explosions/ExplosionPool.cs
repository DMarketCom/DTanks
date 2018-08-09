using SHLibrary.ObjectPool;
using System;
using UnityEngine;

namespace Game.Explosions
{
    public class ExplostionPool : PoolBase<ExplosionEffect>
    {
        private Func<ExplosionEffect> _explosionCreator;

        public ExplostionPool(Func<ExplosionEffect> bulletCreator)
        {
            _explosionCreator = bulletCreator;
        }

        protected override ExplosionEffect CreateObject()
        {
            return _explosionCreator.Invoke();
        }

        public void PlayEffect(Vector3 pos)
        {
            var effect = GetObject() as ExplosionEffect;
            effect.Play(pos, OnEffectPlayEnd);
        }

        private void OnEffectPlayEnd(IPoolObject element)
        {
            AddObjects(element);
        }
    }
}