using DMarketSDK.Market.Items;
using SHLibrary.Logging;
using System;
using System.Collections.Generic;
using DMarketSDK.Domain;
using DMarketSDK.IntegrationAPI.Request;
using DMarketSDK.WidgetCore.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Market.Forms
{
    public class SellOfferForm : WidgetFormViewBase<ItemInfoFormModel>, IFormWithItems
    {
        #region IFormWithItems implementation
        public event Action<ItemActionType, MarketItemModel> ItemClicked;
        #endregion
        public event Action Closed;
        public event Action<long> PriceEntered;

        private MarketItemView TargetItem { get { return _itemView; } }
        private double _currentPriceValue;
        private readonly Price _defaultOfferPrice = new Price(1, Price.DMC);

        [SerializeField]
        private MarketItemView _itemView;
        [SerializeField]
        private List<Button> _btnsClose;
        [SerializeField]
        private TextMeshProUGUI _sellPriceCurrencyText;
        [SerializeField]
        private TMP_InputField _sellPriceInputField;
        [SerializeField]
        private TextMeshProUGUI _txtYouWillGet;     
        [SerializeField]
        private Button _btnCreate;
        [SerializeField]
        private Button _btnEdit;
        
        public override void Show()
        {
            base.Show();
            _sellPriceInputField.text = _defaultOfferPrice.FormatPriceText();
            TargetItem.Show();
        }

        public override void Hide()
        {
            base.Hide();
            TargetItem.Hide();
        }

        public override void ApplyModel(WidgetFormModel model)
        {
            base.ApplyModel(model);
            if (!TargetItem.IsInitialize)
            {
                TargetItem.Initialize();
            }
            TargetItem.ApplyModel(FormModel.CurrentItem);
        }
        
        protected override void OnModelChanged()
        {
            base.OnModelChanged();

            TargetItem.ApplyModel(FormModel.CurrentItem);

            if (FormModel.CurrentItem == null)
            {
                return;
            }

            var difference = (FormModel.CurrentItem.Price - FormModel.CurrentItem.Fee);
            if (difference.Amount < 0)
            {
                difference.Amount = 0;
            }

            _sellPriceInputField.text = FormModel.CurrentItem.Price.FormatPriceText();
            _sellPriceCurrencyText.text = FormModel.CurrentItem.Price.GetCurrencyIconString(_sellPriceCurrencyText.color);
            _txtYouWillGet.text = difference.GetStringWithCurrencySprite(_txtYouWillGet.color);
            _btnCreate.interactable = difference.Amount > 0;

            if (FormModel.Type == ItemActionType.CreateSellOffer)
            {
                _btnCreate.gameObject.SetActive(true);
                _btnEdit.gameObject.SetActive(false);
            }
            else if (FormModel.Type == ItemActionType.EditSellOffer)
            {
                _btnCreate.gameObject.SetActive(false);
                _btnEdit.gameObject.SetActive(true);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _btnsClose.ForEach(button => button.onClick.AddListener(OnCloseClicked));
            TargetItem.Clicked += OnItemClicked;
            _sellPriceInputField.onValueChanged.AddListener(OnPriceChanged);
            _sellPriceInputField.onEndEdit.AddListener(OnPriceEntered);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _btnsClose.ForEach(button => button.onClick.RemoveListener(OnCloseClicked));
            TargetItem.Clicked -= OnItemClicked;
            _sellPriceInputField.onValueChanged.RemoveListener(OnPriceChanged);
            _sellPriceInputField.onEndEdit.RemoveListener(OnPriceEntered);
        }

        private void OnPriceChanged(string value)
        {
            var amount = GetInputAmount(value);
            if (amount == _currentPriceValue)
            {
                return;
            }

            // TODO: need refactoring.
            if (CheckPriceValue(amount))
            {
                amount = _currentPriceValue;
                _sellPriceInputField.text = GetPriceText(amount, value);
                return;
            }
            _currentPriceValue = amount;
            InvokePriceChangeEvent(_currentPriceValue);
        }

        private bool CheckPriceValue(double amount)
        {
            return amount > (PriceExtension.MaxPriceAmount - 1f);
        }

        private void OnPriceEntered(string value)
        {
            if (_currentPriceValue < PriceExtension.MinPriceAmount && _currentPriceValue != 0)
            {
                _currentPriceValue = PriceExtension.MinPriceAmount;
                _sellPriceInputField.text = GetPriceText(_currentPriceValue, value);
                InvokePriceChangeEvent(_currentPriceValue);
            }
        }

        private void InvokePriceChangeEvent(double amount)
        {
            var resultPrice = PriceExtension.ConvertDoubleToAmount(amount);
            PriceEntered.SafeRaise(resultPrice);
        }

        private string GetPriceText(double amount, string textAmount)
        {
            var result = PriceExtension.FormatPriceText(amount);
            var divider = '.';
            if (textAmount.Length > 0 && textAmount[textAmount.Length - 1] == divider)
            {
                result += divider;
            }

            return result;
        }

        private double GetInputAmount(string value)
        {
            double result = 0;
            try
            {
                result = value == string.Empty ? 0 : Convert.ToDouble(value);
                return result;
            }
            catch (Exception e)
            {
                DevLogger.Warning(string.Format("Cannot parse in int: {0}. Exception: {1}", value, e));
            }

            return result;
        }

        private void OnItemClicked(ItemActionType actionType, MarketItemModel model)
        {
            ItemClicked.SafeRaise(actionType, model);
        }

        private void OnCloseClicked()
        {
            Hide();
            Closed.SafeRaise();
        }
    }
}