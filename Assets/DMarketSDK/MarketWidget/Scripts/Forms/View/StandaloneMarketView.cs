using DMarketSDK.Common.UI;
using DMarketSDK.Market.Domain;
using UnityEngine;

namespace DMarketSDK.Market
{
    public sealed class StandaloneMarketView : MarketView
    {
        [SerializeField]
        private TabPanelContainer _tabPanel;

        #region ObserverViewBase code implementaion

        protected override void OnModelChanged()
        {
            
        }

        #endregion

        protected override void Awake()
        {
            base.Awake();
            _tabPanel.BindWithChildTabs();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _tabPanel.Changed += OnActiveTabChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _tabPanel.Changed -= OnActiveTabChanged;
        }

        private void OnActiveTabChanged(int tabIndex)
        {
            CallPanelTabChanged((MarketTabType)tabIndex);
        }

        #region MarketView implementation

        public override void ApplyActivePanelTab(MarketTabType tab)
        {
            _tabPanel.SetActiveTab((int)tab, true);
        }

        public override void SetActiveTabsPanel(bool isActive)
        {
            _tabPanel.gameObject.SetActive(isActive);
        }

        #endregion
    }
}