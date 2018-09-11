using System;
using DG.Tweening;
using SHLibrary;
using SHLibrary.ObjectPool;
using UnityEngine;

namespace Game.Bullet
{
    public class TankBullet : UnityBehaviourBase, IBullet, IPoolObject
    {
        #region IBullet implementation

        public event Action<IBullet, Collider> Hit;

        Vector3 IBullet.Pos { get { return transform.position; } }

        float IBullet.Damage { get { return _currentDamage; } }
        
        int IBullet.UnitID { get { return 0; } }

        void IBullet.Fire(Vector3 startPos, Vector3 endPos, float force)
        {
            const float kMinDamage = 0.9f;
            const float kMaxDamage = 2.1f;
            _currentDamage = UnityEngine.Random.Range(kMinDamage, kMaxDamage);
            _startPos = startPos;
            transform.position = startPos;
            gameObject.SetActive(true);
            _collider.enabled = true;
            endPos.y = startPos.y;
            var duration = 0.2f + Vector3.Distance(_startPos, endPos) / 40f;
            _currentTween = transform.DOMove(endPos, duration);
            _currentTween.SetEase(Ease.Linear);
            _currentTween.OnComplete(OnInTarget);
        }

        #endregion

        #region IPoolObject implementation

        public event Action<IPoolObject> PoolObjectDestroyed;

        protected override void OnDestroyObject()
        {
            base.OnDestroyObject();
            PoolObjectDestroyed.SafeRaise(this);
        }

        void IPoolObject.Activate()
        {
			
        }

        void IPoolObject.Deactivate()
        {
            gameObject.SetActive(false);
            _collider.enabled = false;
            if (_currentTween != null)
            {
                _currentTween.Complete(false);
                _currentTween = null;
            }
        }

        #endregion

        private const float kMinDistanceForActivation = 1f;
        private Vector3 _startPos;
        private float _currentDamage;

        [SerializeField]
        private Collider _collider;

        private Tweener _currentTween;

        private void OnInTarget()
        {
            Hit.SafeRaise(this, null);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Vector3.Distance(transform.position, _startPos) >
                kMinDistanceForActivation)
            {
                Hit.SafeRaise(this, other);
                _currentDamage = 0;
            }
        }
    }
}