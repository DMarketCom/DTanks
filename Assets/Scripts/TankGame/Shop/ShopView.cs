using System;
using Shop;
using SHLibrary.ObserverView;
using SHLibrary.Utils;
using TankGame.Forms;
using TankGame.Shop.Items;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.Shop
{
    public sealed class ShopView : ObserverViewBase<ShopModel>
    {
        public event Action<ShopItemClickType, long> ItemEvent;
        public event Action BackClicked;
        public event Action BasicWidgetClicked;
        public event Action MarketWidgetClicked;

        [SerializeField]
        private MessageBoxForm _messageBoxForm;
        
        [SerializeField]
        public WaitingForm WaitingForm;

        [SerializeField]
        private ShopItemsContainerView _itemsContainer;

        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private Button _basicWidgetButton;

        public MessageBoxForm MessageBoxForm
        {
            get { return _messageBoxForm; }
        }

        public override void Show()
        {
            base.Show();
            _itemsContainer.ApplyModel(Model);
        }

        public void ShowBasicWidgetButton()
        {
            _basicWidgetButton.gameObject.SetActive(true);
        }

        public void HideBasicWidgetButton()
        {
            _basicWidgetButton.gameObject.SetActive(false);
        }

        protected override void OnModelChanged()
        {
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Subscribing(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Subscribing(false);
        }

        private void Subscribing(bool isSubscribe)
        {
            if (isSubscribe)
            {
                _itemsContainer.ItemClicked += OnItemClicked;
                _backButton.onClick.AddListener(OnBackClicked);
                _basicWidgetButton.onClick.AddListener(OnBasicWidgetClicked);
            }
            else
            {
                _itemsContainer.ItemClicked -= OnItemClicked;
                _backButton.onClick.RemoveListener(OnBackClicked);
                _basicWidgetButton.onClick.RemoveListener(OnBasicWidgetClicked);
            }
        }

        private void OnItemClicked(ShopItemModel model, ShopItemClickType actionType)
        {
            ItemEvent.SafeRaise(actionType, model.WorldId);
        }

        private void OnBackClicked()
        {
            BackClicked.SafeRaise();
        }

        private void OnBasicWidgetClicked()
        {
            BasicWidgetClicked.SafeRaise();
        }

        private void OnMarketWidgetClicked()
        {
            MarketWidgetClicked.SafeRaise();
        }
    }
}