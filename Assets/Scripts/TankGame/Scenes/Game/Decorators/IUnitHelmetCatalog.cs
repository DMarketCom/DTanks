using TankGame.Domain.GameItem;
using UnityEngine;

namespace Game.Decorators
{
    public interface IUnitHelmetCatalog 
    {
        GameObject GetHelmet(GameItemType itemType);
    }
}