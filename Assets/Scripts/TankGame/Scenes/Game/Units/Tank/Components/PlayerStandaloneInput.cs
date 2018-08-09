using UnityEngine;
using System;
using SHLibrary;
using System.Collections.Generic;

namespace Game.Units.Components.Standalone
{
    public class PlayerStandaloneInput : UnityBehaviourBase, IUnitInsideInputComponent
    {
        private static readonly Dictionary<KeyCode, Vector2> _keysToDirection;

        static PlayerStandaloneInput()
        {
            _keysToDirection = new Dictionary<KeyCode, Vector2>();
            _keysToDirection.Add(KeyCode.LeftArrow, Vector2.left);
            _keysToDirection.Add(KeyCode.RightArrow, Vector2.right);
            _keysToDirection.Add(KeyCode.UpArrow, Vector2.up);
            _keysToDirection.Add(KeyCode.DownArrow, Vector2.down);
        }

        public event Action<Vector2> Move;
        public event Action<Vector2, float> Fire;

        private const float kMinFireHoldTime = 0.5f;
        private const float kMaxFireHoldTime = 2f;

        private bool IsFirePressed
        {
            get
            {
                return Input.GetKey(KeyCode.Space);
            }
        }

        private float CurrentFireHoldTime
        {
            get
            {
                return Mathf.Clamp(Time.timeSinceLevelLoad - _timeOfFirePress,
                    0, kMaxFireHoldTime);
            }
        }

        private float _timeOfFirePress = 0;

        private void Update()
        {
            if (_timeOfFirePress > 0)
            {
                if (IsFirePressed)
                {
                    if (CurrentFireHoldTime >= kMaxFireHoldTime)
                    {
                        OnFirePressedEnd();
                    }
                }
                else
                {
                    OnFirePressedEnd();
                }
            }
            if (Input.anyKey)
            {
                foreach (var key in _keysToDirection.Keys)
                {
                    if (Input.GetKey(key))
                    {
                        Move.SafeRaise(_keysToDirection[key]);
                    }
                }
                if (IsFirePressed && _timeOfFirePress == 0)
                {
                    //_timeOfFirePress = Time.timeSinceLevelLoad;
                    Fire.SafeRaise(GetFireDirection(), 2f);
                }
            }
        }

        private void OnFirePressedEnd()
        {
            if (CurrentFireHoldTime > kMinFireHoldTime)
            {
                Fire.SafeRaise(GetFireDirection(), CurrentFireHoldTime);
            }
            _timeOfFirePress = 0;
        }
        
        private Vector2 GetFireDirection()
        {
            //TODO need implement
            return Vector2.zero;
        }
    }
}