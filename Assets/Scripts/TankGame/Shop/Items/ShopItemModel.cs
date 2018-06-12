using PlayerData;
using SHLibrary.ObserverView;
using UnityEngine;

namespace Shop
{
    public class ShopItemModel : ObservableBase
    {
        public readonly GameItemType ItemType;
        public readonly Sprite IconSprite;
        public readonly long WorldId;
        public string Name;
        public string Description;
        public bool IsEquipped;
        public bool IsSelected;
        public bool IsInMarket;

        public ShopItemModel(GameItemType itemType, Sprite icon, long worldId)
        {
            ItemType = itemType;
            IconSprite = icon;
            WorldId = worldId;
        }
    }
}