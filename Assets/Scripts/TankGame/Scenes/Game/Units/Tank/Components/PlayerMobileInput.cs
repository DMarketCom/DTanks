using UnityEngine;
using System;
using SHLibrary;

namespace Game.Units.Components.Standalone
{
    public class PlayerMobileInput : UnityBehaviourBase, IUnitInsideInputComponent
    {
        private string HorizontalAxis = "Horizontal";
        private string verticalAxis = "Vertical";
        private string FireKey = "FireButton";

        static PlayerMobileInput()
        {
        }

        public event Action<Vector2> Move;
        public event Action<Vector2, float> Fire;

        private const float kMinFireHoldTime = 0.5f;
        private const float kMaxFireHoldTime = 2f;

        private bool IsFirePressed
        {
            get
            {
                return SimpleInput.GetButtonDown(FireKey);
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
                float inputHorizontal = SimpleInput.GetAxis(HorizontalAxis);
                float inputVertical = SimpleInput.GetAxis(verticalAxis);
                Vector2 directional = new Vector2(inputHorizontal, inputVertical);
                Move.SafeRaise(directional);
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
