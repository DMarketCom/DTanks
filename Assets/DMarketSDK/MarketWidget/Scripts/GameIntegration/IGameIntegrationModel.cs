using System;
using System.Collections.Generic;

namespace DMarketSDK.Market.GameIntegration
{
    public interface IGameIntegrationModel
    {
        event Action ItemsChanged;

        List<InGameItemInfo> Items { get; }
    }
}