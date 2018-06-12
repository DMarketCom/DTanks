using System;
using Shop;
using SHLibrary.ObserverView;
using SHLibrary.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.Shop.Items
{
    public sealed class ShopItemViewFixed : ObserverViewBase<ShopItemModel>
    {
        public event Action<ShopItemModel, ShopItemClickType> Clicked;

        [SerializeField]
        private TextMeshProUGUI[] _txtItemName; // TODO: need change to single component for item name.

        [SerializeField]
        private Image _imgItemIcon;

        [SerializeField]
        private GameObject _selectedItemPanel;

        [SerializeField]
        private Button _btnSelectItem;

        [SerializeField]
        private Button _btnUseItem;

        [SerializeField]
        private Button _btnMarketItem;

        [SerializeField]
        private TextMeshProUGUI _txtMarketButton;

        [SerializeField]
        private TextMeshProUGUI _txtUseButton;

        [SerializeField]
        private GameObject _equippedPanel;

        [SerializeField]
        private Canvas _itemCanvas;

        protected override void OnModelChanged()
        {
            foreach (var txtName in _txtItemName)
            {
                txtName.text = Model.Name;
            }

            _imgItemIcon.sprite = Model.IconSprite;

            _txtUseButton.text = Model.IsEquipped ? "Equipped" : "Equip";
            _btnUseItem.interactable = !Model.IsInMarket && !Model.IsEquipped;

            _equippedPanel.SetActive(Model.IsEquipped && !Model.IsSelected);
            _selectedItemPanel.SetActive(Model.IsSelected);
            _btnMarketItem.interactable = !Model.IsEquipped;
            _txtMarketButton.text = Model.IsInMarket ? "From Market" : "To Market";

            _itemCanvas.overrideSorting = Model.IsSelected;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _btnSelectItem.onClick.AddListener(OnItemSelectClicked);
            _btnUseItem.onClick.AddListener(OnEquipClicked);
            _btnMarketItem.onClick.AddListener(OnMarketClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _btnSelectItem.onClick.RemoveListener(OnItemSelectClicked);
            _btnUseItem.onClick.RemoveListener(OnEquipClicked);
            _btnMarketItem.onClick.RemoveListener(OnMarketClicked);
        }

        private void OnClicked(ShopItemClickType clickType)
        {
            Clicked.SafeRaise(Model, clickType);
        }

        private void OnItemSelectClicked()
        {
            OnClicked(ShopItemClickType.Select);
        }

        private void OnEquipClicked()
        {
            OnClicked(ShopItemClickType.Equip);
        }

        private void OnMarketClicked()
        {
            var clickType = Model.IsInMarket ? ShopItemClickType.FromMarket : ShopItemClickType.ToMarket;
            OnClicked(clickType);
        }
    }
}