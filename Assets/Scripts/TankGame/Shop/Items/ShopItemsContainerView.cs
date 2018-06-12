using System;
using System.Collections.Generic;
using DG.Tweening;
using DMarketSDK.Common.UI;
using PlayerData;
using Shop;
using Shop.SpriteCatalog;
using SHLibrary.ObserverView;
using SHLibrary.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.Shop.Items
{
    public class ShopItemsContainerView : ObserverViewBase<ShopModel>
    {
        public event Action<ShopItemModel, ShopItemClickType> ItemClicked;

        private ShopItemModel _selectedItemModel;

        [SerializeField]
        private ShopItemViewFixed _itemPrefab;

        [SerializeField]
        private ShopImageCatalog _spriteCatalog;

        [SerializeField]
        private GameItemScriptableCatalog _itemInfoCatalog;

        [SerializeField]
        private RectTransform _contentTransform;

        [SerializeField]
        private Button _btnDeselect;

        [SerializeField]
        private int _itemsPerPageCount;

        [SerializeField]
        private float _arrowMoveTime = 0.7f;

        [SerializeField]
        private Ease _animType = Ease.OutExpo;

        [SerializeField]
        private PageIndicatorViewBase _pagination;

        [SerializeField]
        private GridLayoutGroup _contentItemsGrid;

        private IShopImageCatalog SpriteCatalog { get { return _spriteCatalog; } }
        private IGameItemsInfoCatalog InfoCatalog { get { return _itemInfoCatalog; } }

        private int _currentPageIndex;
        private int _totalPagesCount;

        private int ItemWidth
        {
            get { return (int)_contentItemsGrid.cellSize.x; }
        }

        private int RowCount
        {
            get { return _contentItemsGrid.constraintCount; }
        }

        private readonly List<ShopItemViewFixed> _inventoryItems = new List<ShopItemViewFixed>();
        
        protected override void OnEnable()
        {
            base.OnEnable();

            _btnDeselect.onClick.AddListener(OnDeselectItemClick);
            _pagination.PageChangeClicked += OnPageChangeClicked;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _btnDeselect.onClick.RemoveListener(OnDeselectItemClick);
            _pagination.PageChangeClicked -= OnPageChangeClicked;
        }

        private void SetSelectedItemModel(ShopItemModel item)
        {
            if (_selectedItemModel != null && item != _selectedItemModel)
            {
                _selectedItemModel.IsSelected = false;
                _selectedItemModel.SetChanges();
            }
            _selectedItemModel = item;
            if (_selectedItemModel != null)
            {
                _selectedItemModel.IsSelected = true;
                _selectedItemModel.SetChanges();
            }
        }

        private void OnItemClickHandler(ShopItemModel item, ShopItemClickType actionType)
        {
            if (actionType == ShopItemClickType.Select)
            {
                SetSelectedItemModel(item);
            }
            else
            {
                ItemClicked.SafeRaise(item, actionType);
            }
        }

        private void UpdateContentSize()
        {
            var contentWidth = ItemWidth * (_inventoryItems.Count + 1);
            _contentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                contentWidth);
        }

        private void AddNewItem(PlayerItemInfo currentItem)
        {
            var shopItemModel = GetShopItemModel(currentItem);
            var newItem = CreateItem();
            _inventoryItems.Add(newItem);
            newItem.ApplyModel(shopItemModel);
            newItem.Show();
        }

        private void DestroyNotExistingItems()
        {
            var itemsForDelete = new List<ShopItemViewFixed>();
            _inventoryItems.ForEach(item =>
            {
                if (!Model.IsExist(item.Model.WorldId))
                {
                    itemsForDelete.Add(item);
                }
            });
            itemsForDelete.ForEach(item => _inventoryItems.Remove(item));
            itemsForDelete.ForEach(DestroyItem);
        }

        private ShopItemModel GetShopItemModel(PlayerItemInfo currentItem)
        {
            var icon = SpriteCatalog.GetShopItemSprite(currentItem.ItemType);
            var itemModel = new ShopItemModel(currentItem.ItemType,
                icon, currentItem.WorldId);
            var itemInfo = InfoCatalog.GetInfo(currentItem.ItemType);
            itemModel.Name = itemInfo.Name;
            itemModel.Description = itemInfo.Description;
            UpdateModel(currentItem, itemModel);
            return itemModel;
        }

        private void UpdateModel(PlayerItemInfo itemInfo, ShopItemModel itemModel)
        {
            itemModel.IsInMarket = itemInfo.IsInMarket;
            itemModel.IsEquipped = Model.IsEquipped(itemInfo.WorldId);
            itemModel.SetChanges();
        }

        protected override void OnModelChanged()
        {
            DestroyNotExistingItems();

            foreach (var targetInfo in Model.GetItems())
            {
                var info = targetInfo;
                var targetItem = _inventoryItems.Find(item => item.Model.WorldId == info.WorldId);
                if (targetItem == null)
                {
                    AddNewItem(targetInfo);
                }
                else
                {
                    UpdateModel(targetInfo, targetItem.Model);
                }
            }

            UpdateContentSize();

            if (_selectedItemModel != null)
            {
                SetSelectedItemModel(_selectedItemModel);
            }

            _totalPagesCount = CalculateTotalPagesCount();
            _pagination.UpdatePage(_currentPageIndex, _totalPagesCount);
        }

        private ShopItemViewFixed CreateItem()
        {
            var item = Instantiate(_itemPrefab, _contentTransform);
            item.Clicked += OnItemClickHandler;
            return item;
        }

        private void DestroyItem(ShopItemViewFixed item)
        {
            item.Clicked -= OnItemClickHandler;
            item.Hide();
            item.ApplyModel(null);
            Destroy(item.gameObject);
        }

        private void OnDeselectItemClick()
        {
            SetSelectedItemModel(null);
        }

        private void OnPageChangeClicked(int pageIndex)
        {
            _currentPageIndex = pageIndex;
            MoveToPage(pageIndex);
            SetSelectedItemModel(null);
        }

        private void MoveToPage(int pageIndex)
        {
            var moveItemsValue = pageIndex * (_itemsPerPageCount / RowCount);
            var contentPosX = -1 * moveItemsValue * (ItemWidth + _contentItemsGrid.spacing.x);

            _contentTransform.DOLocalMoveX(contentPosX, _arrowMoveTime).SetEase(_animType)
                .OnComplete(() => _pagination.UpdatePage(_currentPageIndex, _totalPagesCount));
        }

        private int CalculateTotalPagesCount()
        {
            return Mathf.CeilToInt((float)_inventoryItems.Count / _itemsPerPageCount);
        }
    }
}