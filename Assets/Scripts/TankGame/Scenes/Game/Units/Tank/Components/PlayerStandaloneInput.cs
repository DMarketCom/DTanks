using UnityEngine;
using System;
using SHLibrary;
using System.Collections.Generic;

namespace Game.Units.Components.Standalone
{
    public class PlayerStandaloneInput : UnityBehaviourBase, IUnitInsideInputComponent
    {
        private readonly Dictionary<KeyCode, Vector2> _keysToDirection;
        
        public event Action<Vector2> Move;
        public event Action<Vector2, float> Fire;

        private const float kMinFireHoldTime = 0.5f;
        private const float kMaxFireHoldTime = 2f;

        private float _timeOfFirePress;

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
                return Mathf.Clamp(Time.timeSinceLevelLoad - _timeOfFirePress, 0, kMaxFireHoldTime);
            }
        }

        protected PlayerStandaloneInput()
        {
            _keysToDirection = new Dictionary<KeyCode, Vector2>
            {
                {KeyCode.LeftArrow, Vector2.left},
                {KeyCode.RightArrow, Vector2.right},
                {KeyCode.UpArrow, Vector2.up},
                {KeyCode.DownArrow, Vector2.down}
            };
        }

        protected override void Update()
        {
            base.Update();
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
                foreach (KeyValuePair<KeyCode, Vector2> keyValue in _keysToDirection)
                {
                    if (Input.GetKey(keyValue.Key))
                    {
                        Move.SafeRaise(keyValue.Value);
                    }
                }

                if (IsFirePressed && _timeOfFirePress == 0)
                {
                    Fire.SafeRaise(Vector2.zero, 2f);
                }
            }
        }

        private void OnFirePressedEnd()
        {
            if (CurrentFireHoldTime > kMinFireHoldTime)
            {
                Fire.SafeRaise(Vector2.zero, CurrentFireHoldTime);
            }
            _timeOfFirePress = 0;
        }
    }
}