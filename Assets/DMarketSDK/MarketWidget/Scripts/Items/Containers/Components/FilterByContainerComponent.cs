using DMarketSDK.Common.UI;
using DMarketSDK.Market.Domain;
using System.Collections.Generic;
using UnityEngine;

namespace DMarketSDK.Market
{
    public class FilterByContainerComponent : ShowingContainerComponentBase
    {
        #region ShowingContainerComponentBase implemenation
        public override void ModifyShowingParams(ref ShowingItemsInfo showingInfo)
        {
            showingInfo.Categories = new List<string>();
            for (var i = 0; i < _activeFilters.Count; i++)
            {
                showingInfo.Categories.Add(_filters[_activeFilters[i]].SearchValue);
            }
        }

        public override void ResetShowingParams(ShowingItemsInfo showingInfo)
        {
            showingInfo.Categories = new List<string>();
            _activeFilters.Clear();
            var categoriesCount = showingInfo.Categories == null ? 0 : showingInfo.Categories.Count;
            if (categoriesCount == 0)
            {
                for (var i = 0; i < _filters.Count; i++)
                {
                    _activeFilters.Add(i);
                }
            }
            else
            {
                for (var i = 0; i < categoriesCount; i++)
                {
                    var targetFilter = _filters.Find(filter => filter.SearchValue.Equals(showingInfo.Categories[i]));
                    if (targetFilter != null)
                    {
                        _activeFilters.Add(_filters.IndexOf(targetFilter));
                    }
                }
            }

            UpdateButtonStates();
        }

        #endregion

        [SerializeField]
        private TabViewBase _tabPrefab;

        [SerializeField]
        private Transform _filtersParentTransform;

        private List<FilterCategory> _filters = new List<FilterCategory>();
        private List<int> _activeFilters = new List<int>();
        private readonly List<TabViewBase> _currentTabs = new List<TabViewBase>();

        public bool HaveFilters { get { return _filters.Count > 0; } }

        public void AddFilters(List<FilterCategory> allFilters, List<int> activeFilters)
        {
            _filters = allFilters;
            _activeFilters = activeFilters;
            DestroyFilterButtons();
            CreateFilterButtons();
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            for (var i = 0; i < _currentTabs.Count; i++)
            {
                _currentTabs[i].SetState(_activeFilters.Contains(i), false);
            }
        }

        private void DestroyFilterButtons()
        {
            foreach (var tab in _currentTabs)
            {
                tab.Clicked -= OnTabClicked;
                Destroy(tab.gameObject);
            }
            _currentTabs.Clear();
        }

        private void CreateFilterButtons()
        {
            for (var i = 0; i < _filters.Count; i++)
            {
                var tab = Instantiate(_tabPrefab, _filtersParentTransform);
                
                var isSelected = _activeFilters.Contains(i);
                tab.SetState(isSelected, false);
                tab.Title = _filters[i].Title;
                tab.Clicked += OnTabClicked;

                _currentTabs.Add(tab);
            }
        }

        private void OnTabClicked(TabViewBase tab)
        {
            var index = _currentTabs.IndexOf(tab);
            if (_activeFilters.Contains(index))
            {
                _activeFilters.Remove(index);
                tab.SetState(false, true);
            }
            else
            {
                _activeFilters.Add(index);
                tab.SetState(true, true);
            }
            ApplyChanging();
        }
    }
}
