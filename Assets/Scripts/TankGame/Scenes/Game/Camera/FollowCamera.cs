﻿using DG.Tweening;
using Game.Tank;
using SHLibrary;
using UnityEngine;

namespace TankGame.GameClient.Camera
{
    public sealed class FollowCamera : UnityBehaviourBase, IFollowCamera
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
        private float _lastUpdateTime;
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
            var distance = Vector3.Distance(_target.Position, transform.position);
            if (distance > _maxDistance)
            {
                if (_tweener != null && _tweener.IsPlaying())
                {
                    _tweener.Kill();
                }
               _tweener = transform.DOMove(_target.Position, _duration);
            }
        }
    }
}
