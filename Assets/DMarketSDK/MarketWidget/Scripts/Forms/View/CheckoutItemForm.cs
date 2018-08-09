using DMarketSDK.Market.Items;
using System;
using System.Collections.Generic;
using DMarketSDK.WidgetCore.Forms;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Market.Forms
{
    public class CheckoutItemForm : WidgetFormViewBase<ItemInfoFormModel>, IFormWithItems
    {
        #region IFormWithItems implementation
        public event Action<ItemActionType, MarketItemModel> ItemClicked;
        #endregion
        public event Action Closed;

        [SerializeField]
        private MarketItemView _item;
        [SerializeField]
        private List<Button> _btnsClose;

        private MarketItemView TargetItem { get { return _item; } }

        protected override void OnModelChanged()
        {
            if (!TargetItem.IsInitialize)
            {
                TargetItem.Initialize();
            }
            TargetItem.ApplyModel(FormModel.CurrentItem);
            TargetItem.Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _btnsClose.ForEach(button => button.onClick.AddListener(OnCloseClicked));
            TargetItem.Clicked += OnItemClicked;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _btnsClose.ForEach(button => button.onClick.RemoveListener(OnCloseClicked));
            TargetItem.Clicked -= OnItemClicked;
        }

        private void OnItemClicked(ItemActionType actionType, MarketItemModel model)
        {
            ItemClicked.SafeRaise(actionType, model);
        }

        private void OnCloseClicked()
        {
            Hide();
            Closed.SafeRaise();
        }
    }
}