using System;
using System.Collections.Generic;
using TankGame.Domain.GameItem;
using UnityEngine;

namespace TankGame.Inventory
{
    public class InventoryImageCatalog : ScriptableObject, IInventoryImageCatalog
    {
        [Serializable]
        protected class ItemImageInfo
        {
            public GameItemType ItemType;
            public Sprite Sprite;
        }

        [SerializeField]
        protected List<ItemImageInfo> ItemId;

        Sprite IInventoryImageCatalog.GetInventoryItemSprite(GameItemType itemId)
        {
            var info = ItemId.Find(item => item.ItemType == itemId);
            if (info != null)
            {
                return info.Sprite;
            }
            return null;
        }
    }
}
