using SHLibrary;
using SHLibrary.ObjectPool;
using SHLibrary.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Explosions
{
    public class ExplosionEffect : UnityBehaviourBase, IPoolObject
    {
        [SerializeField]
        private List<AudioClip> _clips;
        [SerializeField]
        private AudioSource _audio;
        [SerializeField]
        private List<ParticleSystem> _effects;
        
        private Action<IPoolObject> _endCallback;
        private float _playEndTime = 0f;

        #region IPoolObject implementation

        public event Action<IPoolObject> PoolObjectDestroyed;

        void IPoolObject.Activate()
        {
            gameObject.SetActive(true);
        }

        void IPoolObject.Deactivate()
        {
            gameObject.SetActive(false);
        }

        protected override void OnDestroyObject()
        {
            base.OnDestroyObject();
            PoolObjectDestroyed.SafeRaise(this as IPoolObject);
        }

        #endregion

        public void Play(Vector3 pos, Action<IPoolObject> endCallback)
        {
            _endCallback = endCallback;
            transform.position = pos;
            var duration = 0f;
            if (_clips.Count > 0)
            {
                _audio.clip = GetRandom(_clips);
                _audio.Play();
                duration = _audio.clip.length;
            }
            if (_effects.Count > 0)
            {
                var currentEffect = GetRandom(_effects);
                currentEffect.Play();
                duration = Mathf.Max(duration, currentEffect.time);
            }
            _playEndTime = Time.timeSinceLevelLoad + duration;
        }

        private void Update()
        {
            if (_playEndTime > 1f && Time.timeSinceLevelLoad > _playEndTime)
            {
                _playEndTime = 0;
                _endCallback.SafeRaise(this);
            }
        }

        private T GetRandom<T>(List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}