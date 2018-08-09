namespace TankGame.Inventory.Domain
{
    public enum ItemActionType
    {            
        Unknow = -1,
        Buy = 0,
        Sell = 1,
        Equip = 2,
        ToMarket = 3,
        FromMarket = 4,

        /// <summary>
        /// For add item in dev mode
        /// </summary>
        DevTestAdd = 30
    }
}