using System;
using System.Collections.Generic;
using TankGame.Domain.GameItem;
using UnityEngine;

namespace Game.Decorators
{
    public class GameDecorationsCatalog : ScriptableObject, IUnitsSkinCatalog
    {
        #region ISkinDecorator implementation
        Material IUnitsSkinCatalog.GetTankMaterial(GameItemType itemType)
        {
            SelectNewCurrentInfo(itemType);
            return SelectedInfo.TankMaterial;
        }

        Material IUnitsSkinCatalog.GetPickUpMaterial(GameItemType itemType)
        {
            SelectNewCurrentInfo(itemType);
            return SelectedInfo.ItemPickUpMaterial;
        }
        #endregion

        [Serializable]
        public class DecoratorInfo
        {
            public GameItemType ItemType;
            public Material TankMaterial;
            public Material ItemPickUpMaterial;
        }

        [SerializeField]
        protected List<DecoratorInfo> Items;

        private DecoratorInfo SelectedInfo;

        private void SelectNewCurrentInfo(GameItemType itemType)
        {
            SelectedInfo = Items.Find(item => item.ItemType == itemType);
            if (SelectedInfo == null)
            {
                SelectedInfo = new DecoratorInfo();
            }
        }
    }
}