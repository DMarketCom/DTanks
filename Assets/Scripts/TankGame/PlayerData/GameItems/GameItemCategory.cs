using System.Collections.Generic;

namespace PlayerData
{
    public enum GameItemCategory
    {
        None = 0,
        Skin = 1,
        Helmet = 2
    }

    public static class GameItemCategoryExtension
    {
        private readonly static Dictionary<GameItemType, GameItemCategory> _bindedItems = new Dictionary<GameItemType, GameItemCategory>
        {
            {  GameItemType.SkinGreen,     GameItemCategory.Skin },
            {  GameItemType.SkinYellow,    GameItemCategory.Skin },
            {  GameItemType.SkinRed,       GameItemCategory.Skin },
            {  GameItemType.SkinGray,      GameItemCategory.Skin },
            {  GameItemType.SkinBlue,      GameItemCategory.Skin },
            {  GameItemType.SkinFiolet,    GameItemCategory.Skin },
            {  GameItemType.HelmetPropCap,     GameItemCategory.Helmet },
            {  GameItemType.HelmetUnicorn, GameItemCategory.Helmet },
            {  GameItemType.HelmetViking,  GameItemCategory.Helmet },
            {  GameItemType.HelmetGlasses, GameItemCategory.Helmet },
            {  GameItemType.HelmetMilitary, GameItemCategory.Helmet }
        };

        public static GameItemCategory GetItemCategory(GameItemType itemType)
        {
            GameItemCategory itemCategory = GameItemCategory.None;
            _bindedItems.TryGetValue(itemType, out itemCategory);

            return itemCategory;
        }

        public static bool IsValidCategory(GameItemType itemType, GameItemCategory category)
        {
            return GetItemCategory(itemType) == category;
        }
    }
}