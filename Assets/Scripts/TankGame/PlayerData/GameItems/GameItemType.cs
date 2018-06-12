using System.Collections.Generic;

namespace PlayerData
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
        HelmetUnicorn = 8,//TODO delete after reset game server data and market data
        HelmetViking = 9,
        HelmetGlasses = 10,
        HelmetMilitary = 11
    }

    public static class GameItemTypeExtentions
    {
        private readonly static List<GameItemType> _lstAllItems = new List<GameItemType>
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

        static GameItemTypeExtentions()
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
    }
}