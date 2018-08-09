using System;
using DMarketSDK.IntegrationAPI;

namespace TankGame.Inventory.Domain
{
    [Serializable]
    public class ItemsChangingResponse
    {
        //TODO need refactoring: use difference response for marketWidget operation and game item operation
        public string ErrorText;
        public ErrorCode MarketError = ErrorCode.None;

        public bool IsSuccess
        {
            get
            {
                return string.IsNullOrEmpty(ErrorText) &&
                       MarketError == ErrorCode.None;
            }
        }

        public ItemsChangingResponse()
        { }
        
        public ItemsChangingResponse(string error = null)
        {
            ErrorText = error;
        }

        public ItemsChangingResponse(ErrorCode error = ErrorCode.None)
        {
            MarketError = error;
        }
    }
}