using PlayerData;
using UnityEngine;

namespace Game.Decorators
{
    public interface IUnitHelmetCatalog 
    {
        GameObject GetHelmet(GameItemType itemType);
    }
}