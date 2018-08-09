using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Tank;
using SHLibrary.ObserverView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.GameClient.UI
{
    public class TankHealthBarView : ObserverViewBase<TankModel>
    {
        [SerializeField]
        private Image _imgHealthMask;

        [SerializeField]
        private Image _imgHealthColor;

        [SerializeField]
        private TextMeshProUGUI _txtHealthValue;

        [SerializeField]
        private List<HealthColorValue> _healthValues;

        private Tweener _currentTween;
        private float _currentValue = 1;

        private float HealthPercent { get { return Model.Health / Model.MaxHealth; } }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _currentTween.Kill();
        }

        public override void ApplyModel(TankModel model)
        {
            base.ApplyModel(model);

            if (model != null)
            {
                OnUpdateHealthValue(_currentValue);
            }
        }

        protected override void OnModelChanged()
        {
            if (Math.Abs(_currentValue - HealthPercent) > 0.01f)
            {
                UpdateHealthSlider();
            }
        }

        private void UpdateHealthSlider()
        {
            const float animTime = 1.0f;

            if (_currentTween != null)
            {
                _currentTween.Kill(true);
                _currentTween = null;
            }

            float fromValue = _currentValue;
            float toValue = HealthPercent;

            _currentTween = DOVirtual.Float(fromValue, toValue, animTime, OnUpdateHealthValue).SetEase(Ease.Linear);
        }

        private void OnUpdateHealthValue(float value)
        {
            _currentValue = value;
            UpdateUI();
        }

        private void UpdateUI()
        {
            float value = _currentValue;
            _imgHealthColor.color = GetColorByValue(value);
            _imgHealthMask.fillAmount = value;
            _txtHealthValue.text = string.Format("{0}%", Math.Round(100 * value, 0));
        }

        private Color GetColorByValue(float value)
        {
            Color color = Color.clear;

            foreach (var healthColorValue in _healthValues)
            {
                if (value <= healthColorValue.Value)
                {
                    return healthColorValue.Color;
                }
            }

            return color;
        }

        [Serializable]
        private struct HealthColorValue
        {
            public float Value;
            public Color Color;
        }
    }
}