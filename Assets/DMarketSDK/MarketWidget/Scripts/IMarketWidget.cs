using DMarketSDK.Market.GameIntegration;
using System;
using DMarketSDK.Basic;

namespace DMarketSDK.Market
{
    public interface IMarketWidget : IWidgetCore
    {
        event Action<MarketMoveItemRequestParams> MoveItemRequest;

        float Volume { set; get; }

        void Open(IGameIntegrationModel gameItems);
    }
}