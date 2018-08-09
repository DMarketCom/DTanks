using DMarketSDK.Common.UI;
using DMarketSDK.Market.Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Market
{
    public sealed class ItemsPageContainerView : MarketItemsContainer
    {
        private const float _animTime = 0.3f;

        [SerializeField]
        private MarketItemView _itemPrefab;
        [SerializeField]
        private PageIndicatorViewBase _pageIndicator;
        [SerializeField]
        private RectTransform _contentTransform;
        [SerializeField]
        private int _itemsPerPage;
        [SerializeField]
        private float _animDelay = 0f;  

        private readonly List<MarketItemView> _items = new List<MarketItemView>();

        private LayoutGroup _contentLayout;

        public override int CurrentPage { get { return _pageIndicator.CurrentPageIndex; } }

        public override int ItemsPerPage { get { return _itemsPerPage; } }

        public override bool IsInitialize { get { return _contentLayout != null; } }

        protected override void Awake()
        {
            base.Awake();
            _contentLayout = _contentTransform.GetComponent<LayoutGroup>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _pageIndicator.PageChangeClicked += OnChangePageClicked;
            _pageIndicator.UpdateWithoutEvent(0, 1);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _pageIndicator.PageChangeClicked -= OnChangePageClicked;
            _items.ForEach(item => item.Hide());
        }

        public override List<List<MarketItemView>> GetItemsGrid()
        {
            var result = new List<List<MarketItemView>>();
            result.Add(new List<MarketItemView>());
            var currentRow = result[0];
            const float kMinRowDelay = 2f;
            foreach (var item in _items)
            {
                if (!item.IsShowed)
                {
                    continue;
                }

                if (currentRow.Count == 0)
                {
                    currentRow.Add(item);
                    continue;
                }
                var posDifferenceY = item.transform.localPosition.y - currentRow[0].transform.localPosition.y;
                if(Mathf.Abs(posDifferenceY) > kMinRowDelay)
                {
                    result.Add(new List<MarketItemView>());
                    currentRow = result[result.Count - 1];
                }
                currentRow.Add(item);
            }

            return result;
        }

        protected override void OnModelChanged()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            var totalPages = GetTargetTotalPages();
            if (CurrentPage >= totalPages)
            {
                _pageIndicator.UpdateWithoutEvent(totalPages - 1, totalPages);
                UpdateAndNotifyShowingParams();
            }
            else
            {
                _pageIndicator.UpdatePage(CurrentPage, totalPages);
            }
            
            CreateItems();
            UpdateItems();
            UpdateLayout();
        }

        private int GetTargetTotalPages()
        {
            return Mathf.Max(1, Mathf.CeilToInt((float) Model.TotalItemsCount / ItemsPerPage));
        }

        private void OnChangePageClicked(int currentPage)
        {
            _pageIndicator.UpdatePage(currentPage, _pageIndicator.TotalPages);
            UpdateAndNotifyShowingParams();
            UpdateUI();
        }

        private void UpdateItems()
        {
            var targetItems = Model.FilteredItems;

            for (int i = 0; i < _items.Count; i++)
            {
                if (targetItems.Count > i)
                {
                    var isNotSameItem = _items[i].Model == null 
                                        || !_items[i].Model.AssetId.Equals(targetItems[i].AssetId)
                                        || !_items[i].IsShowed;
                    if (isNotSameItem)
                    {
                        _items[i].ShowWithAnim(_animTime, _animDelay);
                        _animDelay += _animTime / 2;
                    }
                    _items[i].ApplyModel(targetItems[i]);
                }
                else
                {
                    _items[i].Hide();
                }
            }
        }

        private void CreateItems()
        {
            if (Model == null)
            {
                return;
            }
            var addItemsCount = _itemsPerPage - _items.Count;
            for (var i = 0; i < addItemsCount; i++)
            {
                _items.Add(CreateItem());
            }
        }

        private void OnItemClicked(ItemActionType actionType, MarketItemModel item)
        {
            CallItemClicked(actionType, item);
        }

        private MarketItemView CreateItem()
        {
            var item = Instantiate(_itemPrefab, _contentTransform);
            item.Initialize();
            item.Clicked += OnItemClicked;
            return item;
        }

        private void DestroyItems()
        {
            _items.ForEach(DestroyItem);
            _items.Clear();
        }

        private void DestroyItem(MarketItemView item)
        {
            item.Clicked -= OnItemClicked;
            item.Hide();
            item.ApplyModel(null);
            Destroy(item.gameObject);
        }

        private void UpdateLayout()
        {
            //fixing nice unity bug for version 2017.1
            if (_contentLayout != null)
            {
                _contentLayout.enabled = false;
                _contentLayout.enabled = true;
            }
        }

        protected override void OnComponentChanged()
        {
            base.OnComponentChanged();
            UpdateUI();
        }
    }
}