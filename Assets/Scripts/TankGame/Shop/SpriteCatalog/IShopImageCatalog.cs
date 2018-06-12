using PlayerData;
using UnityEngine;

namespace Shop.SpriteCatalog
{
    public interface IShopImageCatalog
    {
        Sprite GetShopItemSprite(GameItemType shopItemId);
    }
}