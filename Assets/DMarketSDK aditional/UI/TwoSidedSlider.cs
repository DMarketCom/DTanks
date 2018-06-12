using System;
using UnityEngine;
using SHLibrary;
using SHLibrary.Utils;
using UnityEngine.UI;

namespace DMarketSDK.Common.UI
{
    public sealed class TwoSidedSlider : UnityBehaviourBase
    {
        [SerializeField]
        private Slider _minSlider;

        [SerializeField]
        private Slider _maxSlider;

        private float _minValue;
        private float _maxValue;

        public SliderRangeValue Value { get { return new SliderRangeValue(_minValue, _maxValue); } }

        public event Action<SliderRangeValue> ValueChanged;

        protected override void OnEnable()
        {
            base.OnEnable();

            _maxValue = _maxSlider.maxValue;
            _minSlider.onValueChanged.AddListener(OnLowValueChanged);
            _maxSlider.onValueChanged.AddListener(OnHighValueChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _minSlider.onValueChanged.RemoveListener(OnLowValueChanged);
            _maxSlider.onValueChanged.RemoveListener(OnHighValueChanged);
        }

        private void OnHighValueChanged(float highValue)
        {
            _maxValue = TranslateInverseValue(highValue);
            var clampedValue = Mathf.Clamp(_maxValue, _minValue, _maxSlider.maxValue);
            _maxSlider.value = TranslateInverseValue(clampedValue);

            NotifyValueChanged();
        }

        private void OnLowValueChanged(float lowValue)
        {
            _minValue = lowValue;
            _minSlider.value = Mathf.Clamp(_minValue, _minSlider.minValue, _maxValue);

            NotifyValueChanged();
        }

        private void NotifyValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged.SafeRaise(Value);
            }
        }

        public void SetValues(float minValue, float maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;

            _minSlider.value = minValue;
            _maxSlider.value = TranslateInverseValue(maxValue);
        }

        private float TranslateInverseValue(float value)
        {
            return 1.0f - value;
        }
    }

    public struct SliderRangeValue
    {
        public readonly float MinValue;
        public readonly float MaxValue;

        public SliderRangeValue(float minValue, float maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}