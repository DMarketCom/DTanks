using System;
using DMarketSDK.IntegrationAPI;

namespace DMarketSDK.Market
{

    public class MarketMoveItemResponse
    {
        public readonly string Error;
        public readonly ErrorCode ErrorCode;

        public bool IsSuccess
        {
            get { return String.IsNullOrEmpty(Error); }
        }

        public MarketMoveItemResponse(string error = null, ErrorCode code = ErrorCode.None)
        {
            Error = error;
            ErrorCode = code;
        }
    }
}