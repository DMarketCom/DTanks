using System;
using DMarketSDK.Common.UI;
using DMarketSDK.Market.Domain;
using TMPro;
using UnityEngine;

namespace DMarketSDK.Market.Containers
{
    public sealed class PriceRangeFilterComponent : ShowingContainerComponentBase
    {
        [SerializeField] private TMP_InputField _minPriceInputField;
        [SerializeField] private TMP_InputField _maxPriceInputField;
        [SerializeField] private TwoSidedSlider _priceRangeSlider;
        
        private double _minPriceValue;
        private double _maxPriceValue;

        private double _lowestPrice;
        private double _highestPrice;


        private long _lowestPriceInLong;
        private long _highestPriceInLong;

        protected override void OnEnable()
        {
            base.OnEnable();

            _priceRangeSlider.ValueChanged += OnSliderValueChanged;
            _priceRangeSlider.EndDrag += OnSliderEndDrag;
            _minPriceInputField.onEndEdit.AddListener(OnLowPriceValueChanged);
            _maxPriceInputField.onEndEdit.AddListener(OnHighPriceValueChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _priceRangeSlider.ValueChanged -= OnSliderValueChanged;
            _minPriceInputField.onEndEdit.RemoveListener(OnLowPriceValueChanged);
            _maxPriceInputField.onEndEdit.RemoveListener(OnHighPriceValueChanged);
        }

        #region ShowingContainerComponentBase implementation

        public override void ResetShowingParams(ShowingItemsInfo showingInfo)
        {
            _minPriceValue = PriceExtension.ConvertAmountToDouble(showingInfo.MinPriceRange);
            _maxPriceValue = PriceExtension.ConvertAmountToDouble(showingInfo.MaxPriceRange);
        }

        public override void ModifyShowingParams(ref ShowingItemsInfo showingInfo)
        {
            showingInfo.MinPriceRange = PriceExtension.ConvertDoubleToAmount(_minPriceValue);
            showingInfo.MaxPriceRange = PriceExtension.ConvertDoubleToAmount(_maxPriceValue);
        }

        #endregion

        public void ApplyPriceRange(long minPrice, long maxPrice)
        {
            if (minPrice == _lowestPriceInLong && maxPrice == _highestPriceInLong)
            {
                return;
            }

            _lowestPriceInLong = minPrice;
            _highestPriceInLong = maxPrice;

            _lowestPrice = PriceExtension.ConvertAmountToDouble(minPrice);
            _highestPrice = PriceExtension.ConvertAmountToDouble(maxPrice);

            _minPriceValue = _lowestPrice;
            _maxPriceValue = _highestPrice;

            SetMinPriceText(_lowestPrice);
            SetMaxPriceText(_highestPrice);
            UpdateSliderValues();
        }
        
        private void OnHighPriceValueChanged(string highPriceInput)
        {
            float floatPrice = Convert.ToSingle(highPriceInput);
            _maxPriceValue = Mathf.Clamp(floatPrice, (float)_minPriceValue, (float)_highestPrice);
            SetMaxPriceText(_maxPriceValue);
            UpdateSliderValues();
            ApplyChanging();
        }

        private void OnLowPriceValueChanged(string lowPriceInput)
        {
            var floatPrice = Convert.ToSingle(lowPriceInput);
            _minPriceValue = Mathf.Clamp(floatPrice, (float)_lowestPrice, (float)_highestPrice);

            SetMinPriceText(_minPriceValue);
            UpdateSliderValues();
            ApplyChanging();
        }

        private void OnSliderValueChanged(SliderRangeValue value)
        {
            _minPriceValue = _highestPrice * value.MinValue + _lowestPrice;
            _maxPriceValue = _highestPrice * value.MaxValue;

            SetMinPriceText(_minPriceValue);
            SetMaxPriceText(_maxPriceValue);
        }

        private void OnSliderEndDrag(SliderRangeValue sliderValue)
        {
            ApplyChanging();
        }

        private void SetMinPriceText(double minPrice)
        {
            _minPriceInputField.text = FormatPriceText(minPrice);
        }

        private void SetMaxPriceText(double maxPrice)
        {
            _maxPriceInputField.text = FormatPriceText(maxPrice);
        }

        private void UpdateSliderValues()
        {
            float minNormalized = (float)((_minPriceValue - _lowestPrice) / _highestPrice);
            float maxNormalized = (float)(_maxPriceValue / _highestPrice);
            
            _priceRangeSlider.SetValues(minNormalized, maxNormalized);
        }

        private string FormatPriceText(double priceValue)
        {
            return priceValue.ToString("N");
        }
    }
}
