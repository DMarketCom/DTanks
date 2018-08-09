using TankGame.Domain.GameItem;
using UnityEngine;

namespace TankGame.Inventory
{
    public interface IInventoryImageCatalog
    {
        Sprite GetInventoryItemSprite(GameItemType itemId);
    }
}