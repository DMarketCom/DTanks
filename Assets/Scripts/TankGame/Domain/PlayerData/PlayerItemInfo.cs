using System;
using TankGame.Domain.GameItem;

namespace TankGame.Domain.PlayerData
{
    [Serializable]
    public class PlayerItemInfo
    {
        public long WorldId = -1;

        public GameItemType ItemType = GameItemType.Unknow;

        public bool IsInMarket = false;

        public GameItemCategory ItemCategory
        { get { return GameItemCategoryExtension.GetItemCategory(ItemType); } }

        public PlayerItemInfo() { }
        
        public PlayerItemInfo(GameItemType type, long worldId, bool isInMarket = false)
        {
            ItemType = type;
            WorldId = worldId;
            IsInMarket = isInMarket;
        }
    }
}