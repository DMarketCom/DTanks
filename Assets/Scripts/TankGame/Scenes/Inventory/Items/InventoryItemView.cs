using System;
using SHLibrary.ObserverView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.Inventory.Items
{
    public sealed class InventoryItemView : ObserverViewBase<InventoryItemModel>
    {
        public event Action<InventoryItemModel, InventoryItemClickType> Clicked;

        [SerializeField]
        private TextMeshProUGUI _txtItemNameSelected;

        [SerializeField]
        private TextMeshProUGUI _txtItemName; 

        [SerializeField]
        private Image _imgItemIcon;

        [SerializeField]
        private GameObject _selectedItemPanel;

        [SerializeField]
        private Button _btnSelectItem;

        [SerializeField]
        private Button _btnUseItem;

        [SerializeField]
        private TextMeshProUGUI _txtUseButton;

        [SerializeField]
        private GameObject _equippedPanel;

        protected override void OnModelChanged()
        {
            _txtItemNameSelected.text = Model.Name;
            _txtItemName.text = Model.Name;

            _imgItemIcon.sprite = Model.IconSprite;

            _txtUseButton.text = Model.IsEquipped ? "Equipped" : "Equip";
            _btnUseItem.interactable = !Model.IsInMarket && !Model.IsEquipped;

            _equippedPanel.SetActive(Model.IsEquipped && !Model.IsSelected);
            _selectedItemPanel.SetActive(Model.IsSelected);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _btnSelectItem.onClick.AddListener(OnItemSelectClicked);
            _btnUseItem.onClick.AddListener(OnEquipClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _btnSelectItem.onClick.RemoveListener(OnItemSelectClicked);
            _btnUseItem.onClick.RemoveListener(OnEquipClicked);
        }

        private void OnClicked(InventoryItemClickType clickType)
        {
            Clicked.SafeRaise(Model, clickType);
        }

        private void OnItemSelectClicked()
        {
            OnClicked(InventoryItemClickType.Select);
        }

        private void OnEquipClicked()
        {
            OnClicked(InventoryItemClickType.Equip);
        }
    }
}