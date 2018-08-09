using System.Collections.Generic;
using SHLibrary.ObserverView;
using TankGame.Domain.PlayerData;

namespace TankGame.Inventory
{
    public enum InventoryModeType
    {
        ShowDmarketItems,
        HideDmarketItems
    }

    public class InventorySceneModel : ObservableBase
    {
        public string BasicAccessToken = string.Empty;
        public string BasicRefreshToken = string.Empty;
        public bool IsLoggedDMarket;

        public InventoryModeType Mode = InventoryModeType.ShowDmarketItems;

        private PlayerInventoryInfo _inventory;

        public List<PlayerItemInfo> GetItems()
        {
            var result = new List<PlayerItemInfo>();
            foreach (var item in _inventory.Items)
            {
                bool canShowItem = Mode == InventoryModeType.ShowDmarketItems || !item.IsInMarket;
                //TODO tmp fix
                canShowItem = true;
                if (canShowItem)
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
            return _inventory.IsEquipped(worldId);
        }

        public void SetInventory(PlayerInventoryInfo inventoryInfo)
        {
            _inventory = inventoryInfo;
        }
    }
}