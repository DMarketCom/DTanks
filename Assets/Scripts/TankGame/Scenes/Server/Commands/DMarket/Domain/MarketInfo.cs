using TankGame.Domain.GameItem;

namespace TankGame.GameServer.Commands.DMarket.Domain
{
    public class ItemInfo
    {
        public readonly long WorldId;
        public readonly GameItemType ItemType;

        public ItemInfo(long worldId, GameItemType itemType)
        {
            WorldId = worldId;
            ItemType = itemType;
        }
    }
}
