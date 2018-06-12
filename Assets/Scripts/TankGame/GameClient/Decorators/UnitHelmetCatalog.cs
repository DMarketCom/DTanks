using PlayerData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Decorators
{
    /// <summary>
    /// Contains catalog of helmet items prefab.
    /// </summary>
    [CreateAssetMenu(fileName = "NewHelmetCatalog", menuName = "Create/Catalog/UnitHelmetCatalog")]
    public sealed class UnitHelmetCatalog : ScriptableObject, IUnitHelmetCatalog
    {
        [SerializeField]
        private List<HelmetInfo> _helmetItemsList;

        #region IUnitHelmetCatalog implementation

        public GameObject GetHelmet(GameItemType itemType)
        {
            HelmetInfo info = _helmetItemsList.Find(c => c.ItemType == itemType);
            if(info == null)
            {
                throw new ArgumentException(string.Format("Helmet of type {0} not found in catalog.", itemType));
            }

            return info.HelmetPrefab;
        }

        #endregion

        [Serializable]
        private sealed class HelmetInfo
        {
            public GameItemType ItemType;
            public GameObject HelmetPrefab;
        }
    }
}