using DMarketSDK.Market.Items;
using UnityEngine;

namespace DMarketSDK.Market.Forms
{
    public abstract class InventoryFormBase : TableFormBase<ItemsFormModel>
    {
        [SerializeField]
        private MarketItemView _itemViewPanel;

        protected override void Awake()
        {
            base.Awake();
            _itemViewPanel.Initialize();
        }

        protected override void OnModelChanged()
        {
            base.OnModelChanged();
            UpdateItemViewPanel(FormModel.SelectedItem);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _itemViewPanel.Clicked += OnItemClicked;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _itemViewPanel.Clicked -= OnItemClicked;
            UpdateItemViewPanel(null);
        }

        protected virtual void UpdateItemViewPanel(MarketItemModel item)
        {
            _itemViewPanel.ApplyModel(item);
            if (item != null)
            {
                _itemViewPanel.Show();
            }
            else
            {
                _itemViewPanel.Hide();
            }
        }
    }
}