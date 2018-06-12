using DG.Tweening;
using Game.Tank;
using SHLibrary;
using UnityEngine;

namespace Game.Camera
{
    class FollowCamera : UnityBehaviourBase, IFollowCamera
    {
        #region IFollowCamera implementation
        void IFollowCamera.SetTarget(ITank target)
        {
            _target = target;
        }
        #endregion

        [SerializeField]
        private float _maxDistance = 4f;
        [SerializeField]
        private float _duration = 1f;

        private float _updateInterval = 0.5f;
        private float _lastUpdateTime = 0f;
        private ITank _target;
        private Tweener _tweener;

        private void LateUpdate()
        {
            if (_target != null)
            {
                if (_lastUpdateTime + _updateInterval < Time.timeSinceLevelLoad)
                {
                    FollowTweenUpdate();
                    _lastUpdateTime = Time.timeSinceLevelLoad;
                }
            }
        }

        private void FollowTweenUpdate()
        {
            var distance = Vector3.Distance(_target.Pos, transform.position);
            if (distance > _maxDistance)
            {
                if (_tweener != null && _tweener.IsPlaying())
                {
                    _tweener.Kill(false);
                }
               _tweener = transform.DOMove(_target.Pos, _duration);
            }
        }
    }
}
