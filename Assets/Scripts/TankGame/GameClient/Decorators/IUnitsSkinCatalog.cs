using PlayerData;
using UnityEngine;

namespace Game.Decorators
{
    public interface IUnitsSkinCatalog
    {
        Material GetTankMaterial(GameItemType item);
        Material GetPickUpMaterial(GameItemType item);
    }
}