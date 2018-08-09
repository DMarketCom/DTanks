using System;
using System.Collections.Generic;

namespace DMarketSDK.Market
{
    [Serializable]
    public class MarketMoveItemRequestParams
    {
        public MarketMoveItemType TransactionType;
        public List<string> AssetIds;
        public List<string> ClassIds;
        [NonSerialized]
        public Action<MarketMoveItemResponse> Callback;
    }
}