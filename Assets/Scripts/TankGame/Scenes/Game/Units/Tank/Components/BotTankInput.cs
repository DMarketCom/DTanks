using UnityEngine;
using System;
using SHLibrary;

namespace Game.Units.Components.Standalone
{
    public class BotTankInput : UnityBehaviourBase, IUnitInsideInputComponent
    {
        public event Action<Vector2> Move;
        public event Action<Vector2, float> Fire;

        private const float kMinPauseBetweenFire = 1f;
        private const float kMaxPauseBetweenFire = 5f;

        private const float kMinPauseBeforeChangeDirection = 2f;
        private const float kMaxPauseBeforeChangeDirection = 5f;

        private float _nextFireTime;
        private float _nextChangeDirTime;
        private Vector2 _currentDirection;

        protected override void Update()
        {
            base.Update();
            if (_nextFireTime < Time.timeSinceLevelLoad)
            {
                Fire.SafeRaise(GetRandomDirection(), UnityEngine.Random.value);
                _nextFireTime = UnityEngine.Random.Range(kMinPauseBetweenFire,
                    kMaxPauseBetweenFire);
                _nextFireTime += Time.timeSinceLevelLoad;
            }
            if (_nextChangeDirTime < Time.timeSinceLevelLoad)
            {
                _currentDirection = GetRandomDirection();
                _nextChangeDirTime = UnityEngine.Random.Range
                    (kMinPauseBeforeChangeDirection, kMaxPauseBeforeChangeDirection);
                _nextChangeDirTime += Time.timeSinceLevelLoad;
            }
            Move.SafeRaise(_currentDirection);
        }

        private Vector2 GetRandomDirection()
        {
            return new Vector2(UnityEngine.Random.value, UnityEngine.Random.value);
        }
    }
}