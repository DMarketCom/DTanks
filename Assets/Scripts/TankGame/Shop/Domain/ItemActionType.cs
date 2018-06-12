namespace Shop.Domain
{
    public enum ItemActionType
    {
        Buy,
        Sell,
        Equip,
        ToMarket,
        FromMarket,

        /// <summary>
        /// For add item in dev mode
        /// </summary>
        DevTestAdd = 30
    }
}