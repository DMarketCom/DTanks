using System;

namespace Shop.Domain
{
    public class DMarketLoadDataRequest
    {
        public string MarketToken;
        public Action<DMarketDataLoadResponce> Callback;
    }
}