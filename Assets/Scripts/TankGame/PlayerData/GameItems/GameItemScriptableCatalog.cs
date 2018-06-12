using SHLibrary.Logging;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerData
{
    public class GameItemScriptableCatalog : ScriptableObject, IGameItemsInfoCatalog
    {
        [Serializable]
        public class CatalogItem
        {
            public GameItemType Id;
            public GameItemInfo Info;
        }

        [SerializeField]
        protected List<CatalogItem> Items;

        GameItemInfo IGameItemsInfoCatalog.GetInfo(GameItemType itemId)
        {
            var targetItem = Items.Find(item => item.Id == itemId);
            if (targetItem != null)
            {
                return targetItem.Info;
            }
            else
            {
                DevLogger.Warning(string.Format("Item with id {0} not" +
                    " exist at catolg. Return dafault", itemId));
                return new GameItemInfo();
            }
        }
    }
}