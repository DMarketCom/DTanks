using System;
using System.Collections.Generic;
using DMarketSDK.Domain;
using DMarketSDK.Market.Domain;
using DMarketSDK.Market.Items;

namespace DMarketSDK.Market.Forms
{
    public class ItemsFormModel : WidgetFormModel
    {
        private readonly List<string> _lockedItems = new List<string>();
        private readonly List<MarketItemModel> _marketItems = new List<MarketItemModel>();
        private MarketItemModel _selectedItem;

        public List<MarketItemModel> MarketItems
        {
            get { return _marketItems; }
        }
        
        public ShowingItemsInfo ShowingItemsInfo { get; set; }

        public MarketItemModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetSelectItem(_selectedItem, false);
                _selectedItem = value;
                SetSelectItem(_selectedItem, true);
            }
        }

        private void SetSelectItem(MarketItemModel itemModel, bool isSelected)
        {
            if (itemModel != null)
            {
                itemModel.IsSelected = isSelected;
                itemModel.SetChanges();
            }
        }

        public void RemoveLockAll()
        {
            MarketItems.ForEach(RemoveItemLock);
        }

        public void AddLockAll()
        {
            MarketItems.ForEach(AddItemLock);
        }

        public void AddItemLock(MarketItemModel item)
        {
            if (item != null)
            {
                if (!_lockedItems.Contains(item.AssetId))
                {
                    _lockedItems.Add(item.AssetId);
                }
                item.IsLockFromServer = true;
                item.SetChanges();
            }
        }

        public void RemoveItemLock(MarketItemModel item)
        {
            if (item != null)
            {
                if (_lockedItems.Contains(item.AssetId))
                {
                    _lockedItems.Remove(item.AssetId);
                }
                item.IsLockFromServer = false;
                item.SetChanges();
            }
        }

        public void SetItems(List<MarketItemModel> items)
        {
            var selectedAssetId = SelectedItem != null ? SelectedItem.AssetId : string.Empty;
            var maxCount = Math.Max(MarketItems.Count, items.Count);
            for (var i = 0; i < maxCount; i++)
            {
                if (i >= items.Count)
                {
                    MarketItems.RemoveAt(MarketItems.Count - 1);
                    continue;
                }
                items[i].IsLockFromServer = _lockedItems.Contains(items[i].AssetId);
                if (i >= MarketItems.Count)
                {
                    MarketItems.Add(items[i]);
                }
                else
                {
                    MarketItems[i] = items[i];
                }
                items[i].SetChanges();
            }

            if (MarketItems.Count > 0)
            {
                var newSelectedItem = MarketItems.Find(item => item.AssetId.Equals(selectedAssetId));
                if (newSelectedItem == null)
                {
                    newSelectedItem = MarketItems[0];
                }

                SelectedItem = newSelectedItem;
            }
            else
            {
                SelectedItem = null;
            }
        }
    }
}