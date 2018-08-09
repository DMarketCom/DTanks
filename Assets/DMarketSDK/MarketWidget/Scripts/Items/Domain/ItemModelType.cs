using System;
using System.Collections.Generic;

namespace DMarketSDK.Market.Items
{
    public enum ItemModelType
    {
        None,
        GameInventory,
        MarketInventory,
        MySellOffers,
        BuyItems,
		BuyItem
    }

    public static class ItemModelTypeExtentions
    {
        public static List<ItemModelType> GetAllTypes()
        {
            var result = new List<ItemModelType>();
            var typesCount = Enum.GetValues(typeof(ItemModelType )).Length;
            for (int i = 0; i < typesCount; i++)
            {
                result.Add((ItemModelType)i);
            }
            return result;
        }
    }
}