using System.Collections.Generic;
using System.Linq;
using DMarketSDK.Market.Domain;
using TMPro;
using UnityEngine;

namespace DMarketSDK.Market
{
    public class MobileMarketView : MarketView
    {
        #region MarketView implementation

        public override void ApplyActivePanelTab(MarketTabType tab)
        {
            _marketTabDropdown.value = _dropdownOptions.IndexOf(_dropdownOptions.Find(c => c.TabType == tab));
        }

        public override void SetActiveTabsPanel(bool isActive)
        {
            _marketTabDropdown.gameObject.SetActive(isActive);
        }

        #endregion

        [SerializeField]
        private TMP_Dropdown _marketTabDropdown;

        private List<MarketTabOptionsData> _dropdownOptions;

        private void Awake()
        {
            InitializeDropdown();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _marketTabDropdown.onValueChanged.AddListener(OnMarketTabChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _marketTabDropdown.onValueChanged.RemoveListener(OnMarketTabChanged);
        }

        private void OnMarketTabChanged(int tabIndex)
        {
            MarketTabType tabType = _dropdownOptions[tabIndex].TabType;

            CallPanelTabChanged(tabType);
        }

        private void InitializeDropdown()
        {
            _dropdownOptions = new List<MarketTabOptionsData>();
            foreach (var marketTab in MarketTabs)
            {
                _dropdownOptions.Add(new MarketTabOptionsData(marketTab.Value, marketTab.Key));
            }

            _marketTabDropdown.ClearOptions();
            _marketTabDropdown.AddOptions(_dropdownOptions.Select(c => c.OptionData).ToList());
        }

        /// <summary>
        /// Helper wrapper of Option data for easy working with dropdown.
        /// </summary>
        private struct MarketTabOptionsData
        {
            public readonly TMP_Dropdown.OptionData OptionData;
            public readonly MarketTabType TabType;

            public MarketTabOptionsData(string optionName, MarketTabType tabType)
            {
                OptionData = new TMP_Dropdown.OptionData(optionName);
                TabType = tabType;
            }
        }
    }
}