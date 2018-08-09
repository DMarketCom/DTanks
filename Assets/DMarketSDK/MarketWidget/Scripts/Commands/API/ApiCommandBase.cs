using DMarketSDK.IntegrationAPI;
using System;

namespace DMarketSDK.Market.Commands.API
{
    public class ApiResponse
    {
        public string ErrorTxt;

        public ErrorCode ErrorCode;

        public bool IsSuccessful { get { return string.IsNullOrEmpty(ErrorTxt); } }
        
        public ApiResponse(string error = null)
        {
            ErrorTxt = error;
        }
    }

    public abstract class ApiCommandBase : MarketCommandBase
    {
        protected ClientApi MarketApi { get { return Controller.MarketApi; } }

        public ApiResponse Response { get; private set; }

        public override void Start()
        {
            base.Start();
            Response = new ApiResponse();
        }

        protected void OnError(Error error)
        {
            Response.ErrorCode = error.ErrorCode;
            OnError(Controller.ErrorHelper.GetErrorMessage(error.ErrorCode));
        }

        protected virtual void OnError(string error, ErrorCode code = ErrorCode.None)
        {
            Response.ErrorCode = code;
            Response.ErrorTxt = error;
            Terminate(false);
        }

        protected DateTime IntToDateTime(float intDate)
        {
            var result = new DateTime(1970, 1, 1);
            result = result.Add(TimeZone.CurrentTimeZone.GetUtcOffset(result));
            result = result.Add(TimeSpan.FromSeconds(intDate));
            return result;
        }
    }
}