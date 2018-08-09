using System;
using UnityEngine;
using SHLibrary;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DMarketSDK.Common.UI
{
    public sealed class TwoSidedSlider : UnityBehaviourBase
    {
        [SerializeField]
        private Slider _minSlider;
        [SerializeField]
        private Slider _maxSlider;

        [SerializeField]
        private EventTrigger _minSliderTrigger;
        [SerializeField]
        private EventTrigger _maxSliderTrigger;

        private float _minValue;
        private float _maxValue;

        public SliderRangeValue Value { get { return new SliderRangeValue(_minValue, _maxValue); } }

        public event Action<SliderRangeValue> ValueChanged;
        public event Action<SliderRangeValue> EndDrag; 

        protected override void OnEnable()
        {
            base.OnEnable();

            _maxValue = _maxSlider.maxValue;
            _minSlider.onValueChanged.AddListener(OnLowValueChanged);
            _maxSlider.onValueChanged.AddListener(OnHighValueChanged);
            SubscribeToEndDragEvent();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _minSlider.onValueChanged.RemoveListener(OnLowValueChanged);
            _maxSlider.onValueChanged.RemoveListener(OnHighValueChanged);
            UnsubscribeFromEndDragEvent();
        }

        private void SubscribeToEndDragEvent()
        {
            EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
            EventTriggerType triggerType = EventTriggerType.EndDrag;
            triggerEvent.AddListener(OnSliderEndDrag);

            EventTrigger.Entry entryTrigger = new EventTrigger.Entry
            {
                callback = triggerEvent,
                eventID = triggerType
            };

            _minSliderTrigger.triggers.Add(entryTrigger);
            _maxSliderTrigger.triggers.Add(entryTrigger);
        }

        private void UnsubscribeFromEndDragEvent()
        {
            _minSliderTrigger.triggers.Clear();
            _maxSliderTrigger.triggers.Clear();
        }

        private void OnHighValueChanged(float highValue)
        {
            if (Math.Abs(highValue - _maxValue) > float.Epsilon)
            {
                _maxValue = TranslateInverseValue(highValue);
                _maxValue = Mathf.Clamp(_maxValue, _minValue, _maxSlider.maxValue);
                _maxSlider.value = TranslateInverseValue(_maxValue);

                NotifyValueChanged();
            }
        }

        private void OnLowValueChanged(float lowValue)
        {
            if (Math.Abs(lowValue - _minValue) > float.Epsilon)
            {
                _minValue = Mathf.Clamp(lowValue, _minSlider.minValue, _maxValue);
                _minSlider.value = _minValue;
                NotifyValueChanged();
            }
        }

        private void OnSliderEndDrag(BaseEventData eventData)
        {
            EndDrag.SafeRaise(Value);
        }

        private void NotifyValueChanged()
        {
            ValueChanged.SafeRaise(Value);
        }

        public void SetValues(float minValue, float maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;

            _minSlider.value = _minValue;
            _maxSlider.value = TranslateInverseValue(maxValue);
        }

        private float TranslateInverseValue(float value)
        {
            return 1.0f - value;
        }
    }
}