using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DMarketSDK.Common.UI;
using DMarketSDK.Market.Items;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Market
{
    public sealed class ItemsScrollContainerView : MarketItemsContainer
    {
        #region MarketItemsContainer implementation

        public override int CurrentPage { get { return _pagination.CurrentPageIndex; } }
        public override int ItemsPerPage { get { return _itemsPerPage; } }
        public override bool IsInitialize { get { return true; } }

        public override List<List<MarketItemView>> GetItemsGrid()
        {
            return new List<List<MarketItemView>>();
        }

        #endregion
        
        [SerializeField]
        private ScrollRect _scrollRect;
        [SerializeField]
        private RectTransform _itemsContent;
        [SerializeField]
        private ContentSizeFitter _scrollContentFilter;
        [SerializeField]
        private ContentSizeFitter _itemsContentFilter;
        [SerializeField]
        private MarketItemView _itemPrefab;
        [SerializeField]
        private LoadingSpinner _loadingSpinner;

        [SerializeField]
        private PageIndicatorViewBase _pagination;
        [SerializeField]
        private int _itemsPerPage;

        [SerializeField]
        private RectTransform _searchPanelTransform;
        [SerializeField]
        private float _collapsedHeight;
        [SerializeField]
        private float _expandedHeight;
        [SerializeField]
        private float _collapsedElementsOffset;

        private bool _isLoading;
        private Coroutine _updateCoroutine;
        private RectTransform _rectTransform;
        private Tweener _resizeTween;
        private MarketView _marketView;
        private const float CollapseAnimationDuration = 0.3f;

        private readonly List<MarketItemView> _itemViews = new List<MarketItemView>();
        private readonly Queue<MarketItemView> _marketItemsPool = new Queue<MarketItemView>();

        private List<MarketItemModel> ItemModels { get { return Model.FilteredItems; } }
        private bool IsCollapsePossible { get { return _scrollRect.content.rect.height > _expandedHeight; } }

        private bool IsResizingNow { get { return _resizeTween != null && _resizeTween.IsPlaying(); } }
        private bool IsFullScroll { get { return _rectTransform.sizeDelta.y > _expandedHeight - 10f; } }

        protected override void Awake()
        {
            base.Awake();

            _marketView = FindObjectOfType<MarketView>();
            _rectTransform = transform as RectTransform;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
            _pagination.UpdatePage(0, 1);
            _pagination.PageChangeClicked += OnPageChanged;
            ResetScrollRect();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _pagination.PageChangeClicked -= OnPageChanged;
            _scrollRect.onValueChanged.RemoveListener(OnScrollValueChanged);
            HideItemsViews();

            if (_updateCoroutine != null)
            {
                StopCoroutine(_updateCoroutine);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DestroyItems();
        }

        protected override void OnModelChanged()
        {
            UpdatePaginationUI();

            _updateCoroutine = StartCoroutine(WaitAndUpdateItemsList());
        }

        protected override void OnComponentChanged()
        {
            base.OnComponentChanged();
            UpdateUI();
        }

        private IEnumerator WaitAndUpdateItemsList()
        {
            if (_isLoading)
            {
                yield return new WaitForSeconds(0.5f);
                SetLoading(false);
                SetScrollToStart();
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            if (ItemModels.Count == 0)
            {
                HideItemsViews();
                return;
            }

            if (_itemViews.Count > ItemModels.Count)
            {
                int itemsForHideCount = _itemViews.Count - ItemModels.Count;
                for (int i = 0; i < itemsForHideCount; i++)
                {
                    var itemView = _itemViews.First();
                    _itemViews.Remove(itemView);
                    AddItemToPool(itemView);
                }
            }

            for (int i = 0; i < ItemModels.Count; i++)
            {
                MarketItemView item;
                if (i < _itemViews.Count)
                {
                    item = _itemViews[i];
                }
                else
                {
                    item = GetItemFromPool();
                    _itemViews.Add(item);
                }

                item.ApplyModel(ItemModels[i]);
            }
            
            RebuildContentSizeFilters();
        }

        private void UpdatePaginationUI()
        {
            bool isPaginationActive = Model.TotalItemsCount != Model.FilteredItems.Count;
            _pagination.gameObject.SetActive(isPaginationActive);

            var totalPages = CalculateTotalPagesCount();
            if (CurrentPage >= totalPages)
            {
                _pagination.UpdateWithoutEvent(totalPages - 1, totalPages);
                UpdateAndNotifyShowingParams();
            }
            else
            {
                _pagination.UpdatePage(CurrentPage, totalPages);
            }
        }

        private MarketItemView CreateItemInstance()
        {
            MarketItemView item = Instantiate(_itemPrefab, _itemsContent);
            item.Initialize();
            item.Clicked += OnItemClicked; 
            return item;
        }

        private void HideItemsViews()
        {
            foreach (var marketItemView in _itemViews)
            {
                AddItemToPool(marketItemView);
            }

            _itemViews.Clear();

            RebuildContentSizeFilters();
        }

        private void DestroyItems()
        {
            foreach (var marketItemView in _marketItemsPool)
            {
                marketItemView.Clicked -= OnItemClicked;
            }
        }

        private void AddItemToPool(MarketItemView item)
        {
            item.Hide();

            _marketItemsPool.Enqueue(item);
        }

        private MarketItemView GetItemFromPool()
        {
            var item = _marketItemsPool.Any() 
                ? _marketItemsPool.Dequeue() 
                : CreateItemInstance();
            item.RectTransform.SetAsLastSibling();
            item.Show();

            return item;
        }

        private void OnPageChanged(int page)
        {
            SetLoading(true);
            _pagination.UpdatePage(page, CalculateTotalPagesCount());
            UpdateAndNotifyShowingParams();
        }

        private void OnItemClicked(ItemActionType actionType, MarketItemModel itemModel)
        {
            CallItemClicked(actionType, itemModel);
        }

        private void RebuildContentSizeFilters()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollContentFilter.transform as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_itemsContentFilter.transform as RectTransform);
        }

        private void ResetScrollRect()
        {
            _scrollRect.verticalNormalizedPosition = 1;
        }

        private int CalculateTotalPagesCount()
        {
            return Mathf.Max(1, Mathf.CeilToInt((float)Model.TotalItemsCount / ItemsPerPage));
        }

        private void SetScrollToStart()
        {
            _scrollRect.normalizedPosition = new Vector2(0, 1);
        }

        private void SetLoading(bool isLoading)
        {
            _isLoading = isLoading;
            _loadingSpinner.SetActiveSpinner(_isLoading);
        }

        private void OnScrollValueChanged(Vector2 scrollValue)
        {
            if (IsResizingNow)
            {
                return;
            }

            bool mustBeFull = _scrollRect.content.localPosition.y > 110f;
            if (mustBeFull && !IsFullScroll && IsCollapsePossible)
            {
                ChangeScreenMode(true);
            }
            else if (!mustBeFull && IsFullScroll)
            {
                ChangeScreenMode(false);
            }
        }

        private void ChangeScreenMode(bool toFullScroll)
        {
            var targetHeight = toFullScroll ? _expandedHeight : _collapsedHeight;
            var targetSize = new Vector2(_rectTransform.sizeDelta.x, targetHeight);
            _resizeTween = _rectTransform.DOSizeDelta(targetSize, CollapseAnimationDuration);

            if (IsFullScroll)
            {
                _marketView.ShowHeaders(CollapseAnimationDuration);
                ChangeSearchCollapse(-_collapsedElementsOffset, CollapseAnimationDuration);
            }
            else
            {
                _marketView.HideHeaders(CollapseAnimationDuration);
                ChangeSearchCollapse(_collapsedElementsOffset, CollapseAnimationDuration);
            }
        }

        private void ChangeSearchCollapse(float deltaYValue, float animDuration)
        {
            if(_searchPanelTransform == null)
                return;
            _searchPanelTransform.DOAnchorPosY(_searchPanelTransform.anchoredPosition.y + deltaYValue,
                animDuration);
        }
    }
}