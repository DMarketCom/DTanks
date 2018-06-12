using SHLibrary.Logging;
using System;
using System.Collections.Generic;

namespace PlayerData
{
    [Serializable]
    public class PlayerInventoryInfo
    {
        public int Coins = 100;
        
        public List<PlayerItemInfo> Items = new List<PlayerItemInfo>();
        
        public List<long> EquippedItems = new List<long>();
        
        public PlayerInventoryInfo() { }
        
        public void AddItem(PlayerItemInfo item)
        {
            Items.Add(item);
        }
        
        public void AddItem(GameItemType itemType, long worldId)
        {
            AddItem(new PlayerItemInfo(itemType, worldId));
        }

        public bool RemoveItem(long worldId)
        {
            var result = GetItem(worldId);
            if (result != null)
            {
                Items.Remove(result);
                return true;
            }
            return false;
        }
        
        public PlayerItemInfo GetItem(long worldId)
        {
            return Items.Find(item => item.WorldId == worldId);
        }

        public void EquipItem(long worldId)
        {
            if (EquippedItems.Contains(worldId))
            {
                DevLogger.Error(string.Format("Item with Id:{0} already equipped.", worldId));
                return;
            }
            PlayerItemInfo item = GetItem(worldId);
            TakeOffEquipItems(item.ItemCategory);
            EquippedItems.Add(worldId);
        }
        
        public void EquipItem(PlayerItemInfo itemInfo)
        {
            EquipItem(itemInfo.WorldId);
        }
        
        public void TakeOffEquipItems(GameItemCategory category)
        {
            EquippedItems.RemoveAll(c => GetItem(c).ItemCategory == category);
        }
        
        public bool IsEquipped(long worldId)
        {
            return EquippedItems.Contains(worldId);
        }
        
        public List<GameItemType> GetEquippedItemsTypes()
        {
            List<GameItemType> itemsList = new List<GameItemType>();
            foreach (var itemId in EquippedItems)
            {
                itemsList.Add(GetItem(itemId).ItemType);
            }

            return itemsList;
        }

        public void RemoveAllDMakretItems()
        {
            var dmarketItems = Items.FindAll(item => item.IsInMarket == true);
            dmarketItems.ForEach(item => Items.Remove(item));
        }
    }
}