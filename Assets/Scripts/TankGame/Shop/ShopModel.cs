using PlayerData;
using SHLibrary.ObserverView;
using System.Collections.Generic;

namespace Shop
{
    public enum ShopModeType
    {
        ShowDMarketItems,
        NotShowDmarketItems
    }

    public class ShopModel : ObservableBase
    {
        public string GameToken = string.Empty;
        public string GameRefreshToken = string.Empty;
        public ShopModeType Mode = ShopModeType.ShowDMarketItems;

        public PlayerInventoryInfo Inventory { set; private get; }

        public List<PlayerItemInfo> GetItems()
        {
            var result = new List<PlayerItemInfo>();
            foreach (var item in Inventory.Items)
            {
                if (Mode != ShopModeType.NotShowDmarketItems
                    || !item.IsInMarket)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public bool IsExist(long worldId)
        {
            return GetItems().Find(item => item.WorldId == worldId) != null;
        }

        public bool IsEquipped(long worldId)
        {
            return Inventory.IsEquipped(worldId);
        }
    }
}