using System;

namespace TankGame.Inventory.Domain
{
    public class DMarketLoadDataRequest
    {
        public string MarketToken;
        public Action<DMarketDataLoadResponse> Callback;
    }
}