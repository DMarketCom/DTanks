using TankGame.Domain.GameItem;

namespace TankGame.Catalogs.Game
{
    public interface IGameItemsInfoCatalog
    {
        GameItemInfo GetInfo(GameItemType itemId);
    }
}
