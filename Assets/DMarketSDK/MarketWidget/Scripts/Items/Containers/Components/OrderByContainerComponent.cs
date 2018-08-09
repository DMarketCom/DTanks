using DMarketSDK.Market.Domain;
using System.Collections.Generic;
using DMarketSDK.Market.Components;
using UnityEngine;

namespace DMarketSDK.Market
{
    public class OrderByContainerComponent : OrderByComponentBase
    {
        #region ShowingContainerComponentBase implemenation

        public override void ResetShowingParams(ShowingItemsInfo showingInfo)
        {
            if (Filters != null)
            {
                _currentOrderType = showingInfo.OrderByDirection;
                _searchValue = showingInfo.OrderBy;
                UpdateOrderByButtons();
            }
        }

        public override void ModifyShowingParams(ref ShowingItemsInfo showingInfo)
        {
            if (!string.IsNullOrEmpty(_searchValue))
            {
                showingInfo.OrderByDirection = _currentOrderType;
                showingInfo.OrderBy = _searchValue;
            }
        }
        #endregion

        [SerializeField]
        private List<OrderByButton> _currentButtons = new List<OrderByButton>();

        private OrderDirectionType _currentOrderType = OrderDirectionType.None;
        private string _searchValue = string.Empty;

        protected override void OnStart()
        {
            UpdateOrderByButtons();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _currentButtons.ForEach(button => button.Clicked += OnOrderClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _currentButtons.ForEach(button => button.Clicked -= OnOrderClicked);
        }

        private void UpdateOrderByButtons()
        {
            foreach(var orderBtn in _currentButtons)
            {
                orderBtn.Interactable = !string.IsNullOrEmpty(GetSearchValue(orderBtn));
                orderBtn.SetState(OrderDirectionType.None);
            }
        }

        private void OnOrderClicked(OrderByButton button, OrderDirectionType order)
        {
            _currentOrderType = order;
            _searchValue = GetSearchValue(button);
            ApplyChanging();
        }

        private string GetSearchValue(OrderByButton button)
        {
            var filter = Filters.Find(item => item.Title == button.Title);
            return filter != null ? filter.SearchValue : string.Empty;
        }
    }
}