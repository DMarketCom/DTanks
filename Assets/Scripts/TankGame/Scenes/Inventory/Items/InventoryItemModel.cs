using SHLibrary.ObserverView;
using TankGame.Domain.GameItem;
using UnityEngine;

namespace TankGame.Inventory.Items
{
    public class InventoryItemModel : ObservableBase
    {
        public readonly GameItemType ItemType;
        public readonly Sprite IconSprite;
        public readonly long WorldId;
        public string Name;
        public string Description;
        public bool IsEquipped;
        public bool IsSelected;
        public bool IsInMarket;

        public InventoryItemModel(GameItemType itemType, Sprite icon, long worldId)
        {
            ItemType = itemType;
            IconSprite = icon;
            WorldId = worldId;
        }
    }
}