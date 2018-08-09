using TankGame.Domain.GameItem;

namespace Game.Units.Components
{
    public interface IUnitSkinDecorator
    { 
        void ApplySkinItem(GameItemType item);
    }
}