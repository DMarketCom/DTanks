using UnityEngine;
using System.Collections.Generic;
using DMarketSDK.Market.Domain;
using TMPro;
using OptionData = TMPro.TMP_Dropdown.OptionData;
using System.Globalization;

namespace DMarketSDK.Market.Components
{
    public class ItemOrderByDropdownComponent :  OrderByComponentBase
    {
        private class OrderDropdownItem
        {
            public string Title;
            public string SearchValue;
            public OrderDirectionType Direction;
        }

        #region ShowingContainerComponentBase implmentation

        public override void ResetShowingParams(ShowingItemsInfo showingInfo)
        {
            if (_lstDropItems.Count == 0)
            {
                return;
            }

            var targetDropItem = _lstDropItems.Find(item => item.SearchValue == showingInfo.OrderBy
                                                            && item.Direction == showingInfo.OrderByDirection);
            _orderByDropdown.value = targetDropItem != null ? _lstDropItems.IndexOf(targetDropItem) : 0;
        }

        public override void ModifyShowingParams(ref ShowingItemsInfo showingInfo)
        {
            if (_lstDropItems.Count != 0)
            {
                var currentDropItem = _lstDropItems[_orderByDropdown.value];
                showingInfo.OrderByDirection = currentDropItem.Direction;
                showingInfo.OrderBy = currentDropItem.SearchValue;
            }
        }

        #endregion

        #region OrderByComponentBase implementation

        protected override void OnStart()
        {
            CreateDropItems();
            UpdateDropdownValues();
        }

        #endregion

        [SerializeField]
        private TMP_Dropdown _orderByDropdown;

        private List<OrderDropdownItem> _lstDropItems = new List<OrderDropdownItem>();
        
        protected override void OnEnable()
        {
            _orderByDropdown.onValueChanged.AddListener(OnOrderValueChanged);
        }

        protected override void OnDisable()
        {
            _orderByDropdown.onValueChanged.RemoveListener(OnOrderValueChanged);
        }

        private void CreateDropItems()
        {
            _lstDropItems = new List<OrderDropdownItem>();
            foreach (var filter in Filters)
            {
                AddDropItem(filter, OrderDirectionType.Asc);
                AddDropItem(filter, OrderDirectionType.Desc);
            }
        }

        private void UpdateDropdownValues()
        {
            bool needShow = _lstDropItems.Count > 0;
            _orderByDropdown.gameObject.SetActive(needShow);
            if (needShow)
            {
                _orderByDropdown.ClearOptions();
                var dropDownContent = new List<OptionData>();
                foreach (var item in _lstDropItems)
                {
                    dropDownContent.Add(new OptionData(item.Title));
                }
                _orderByDropdown.AddOptions(dropDownContent);
                _orderByDropdown.value = 0;
            }
        }

        private void OnOrderValueChanged(int valueIndex)
        {
            ApplyChanging();
        }

        private void AddDropItem(FilterCategory filter, OrderDirectionType direction)
        {
            _lstDropItems.Add(new OrderDropdownItem()
            {
                Title = GetTitle(filter.SearchValue, direction),
                SearchValue = filter.SearchValue,
                Direction = direction
            });
        }

        private string GetTitle(string searchValue, OrderDirectionType direction)
        {
            var orderTarget = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(searchValue.ToLower());
            var orderDir = direction == OrderDirectionType.Asc ? "Lower" : "Higher";
            return string.Format("Sort by {0}: {1} First", orderTarget, orderDir);
        }
    }
}