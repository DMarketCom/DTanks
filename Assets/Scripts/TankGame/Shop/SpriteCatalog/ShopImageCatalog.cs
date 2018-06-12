using PlayerData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shop.SpriteCatalog
{
    public class ShopImageCatalog : ScriptableObject, IShopImageCatalog
    {
        [Serializable]
        public class ItemImageInfo
        {
            public GameItemType ItemType;
            public Sprite Sprite;
        }

        [SerializeField]
        protected List<ItemImageInfo> ItemId;

        Sprite IShopImageCatalog.GetShopItemSprite(GameItemType shopItemId)
        {
            var info = ItemId.Find(item => item.ItemType == shopItemId);
            if (info != null)
            {
                return info.Sprite;
            }
            return null;
        }
    }
}
