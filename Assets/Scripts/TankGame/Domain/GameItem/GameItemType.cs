using System.Collections.Generic;

namespace TankGame.Domain.GameItem
{
    public enum GameItemType
    {
        Unknow = 0,

        SkinGreen =1,
        SkinYellow = 2,
        SkinRed = 3,
        SkinBlue = 4,
        SkinGray = 5,
        SkinFiolet = 6,

        HelmetPropCap = 7,
        HelmetViking = 9,
        HelmetGlasses = 10,
        HelmetMilitary = 11
    }

    public static class GameItemTypeExtensions
    {
        private static readonly List<GameItemType> _lstAllItems = new List<GameItemType>
            {
                GameItemType.SkinGreen,
                GameItemType.SkinYellow,
                GameItemType.SkinRed,
                GameItemType.SkinGray,
                GameItemType.SkinBlue,
                GameItemType.SkinFiolet,
                GameItemType.HelmetPropCap,
                GameItemType.HelmetViking,
                GameItemType.HelmetGlasses,
                GameItemType.HelmetMilitary
            };

        private static readonly Dictionary<GameItemCategory, List<GameItemType>> _itemByCategory;

        static GameItemTypeExtensions()
        {
            _itemByCategory = new Dictionary<GameItemCategory, List<GameItemType>>();
            foreach (var item in _lstAllItems)
            {
                var category = GameItemCategoryExtension.GetItemCategory(item);
                if (!_itemByCategory.ContainsKey(category))
                {
                    _itemByCategory.Add(category, new List<GameItemType>());
                }
                _itemByCategory[category].Add(item);
            }
        }

        public static GameItemType GetRandomItem(GameItemCategory category)
        {
            return _itemByCategory[category][UnityEngine.Random.Range(0, _itemByCategory[category].Count)];
        }

        public static List<GameItemType> GetAvailableItemTypes()
        {
            return _lstAllItems;
        }
    }
}