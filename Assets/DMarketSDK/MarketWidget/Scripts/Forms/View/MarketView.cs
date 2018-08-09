using System;
using System.Collections.Generic;
using DG.Tweening;
using DMarketSDK.Forms;
using DMarketSDK.Market.Domain;
using DMarketSDK.WidgetCore.Forms;
using UnityEngine;

namespace DMarketSDK.Market
{
    public abstract class MarketView : WidgetFormViewBase<WidgetModel>
    {
        protected readonly Dictionary<MarketTabType, string> MarketTabs = new Dictionary<MarketTabType, string>
        {
            {MarketTabType.InGameInventory, "In Game Inventory"},
            {MarketTabType.DmarketInventory, "DMarket Inventory"},
            {MarketTabType.MySellOffers, "My Sell Offers"},
            {MarketTabType.BuyItems, "Buy Items"},
        };

        public event Action<MarketTabType> PanelTabChanged;
        public event Action LogoutClicked;
        public event Action CloseClicked;

        [SerializeField] private MarketHeaderView _marketHeader;
        [SerializeField] private float _collapsedElementsOffset;
        [SerializeField] private List<RectTransform> _collapsedTransforms;

        public IMarketHeader Header
        {
            get { return _marketHeader; }
        }

        public void ApplyModel(WidgetModel marketModel)
        {
            base.ApplyModel(marketModel);

            _marketHeader.ApplyModel(marketModel);
        }

        public void HideHeaders(float duration)
        {
            ChangeHeaderState(duration, _collapsedElementsOffset);
        }

        public void ShowHeaders(float duration)
        {
            ChangeHeaderState(duration, -_collapsedElementsOffset);
        }

        private void ChangeHeaderState(float duration, float deltaPosY)
        {
            foreach (var header in _collapsedTransforms)
            {
                header.DOAnchorPosY(header.anchoredPosition.y + deltaPosY, duration);
            }
        }

        public abstract void ApplyActivePanelTab(MarketTabType tab);

        public abstract void SetActiveTabsPanel(bool isActive);

        protected override void OnEnable()
        {
            base.OnEnable();

            Header.ClosedClicked += OnCloseClicked;
            Header.LogoutClicked += OnLogoutClicked;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Header.ClosedClicked -= OnCloseClicked;
            Header.LogoutClicked -= OnLogoutClicked;
        }

        protected void CallPanelTabChanged(MarketTabType tabType)
        {
            PanelTabChanged.SafeRaise(tabType);
        }

        private void OnLogoutClicked()
        {
            LogoutClicked.SafeRaise();
        }

        private void OnCloseClicked()
        {
            CloseClicked.SafeRaise();
        }
    }
}