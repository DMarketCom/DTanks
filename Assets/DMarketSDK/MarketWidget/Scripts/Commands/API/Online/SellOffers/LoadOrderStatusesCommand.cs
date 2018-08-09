using DMarketSDK.IntegrationAPI.Request.MarketIntegration;
using SHLibrary.Logging;
using System.Collections.Generic;

namespace DMarketSDK.Market.Commands.API
{
    public class LoadOrderStatusesCommand : LoadFiltersCommandBase
    {

        public override void Start()
        {
            base.Start();
            MarketApi.GetOrderStatuses(OnSuccesCallback, OnError);
        }

        private void OnSuccesCallback(GetSellOfferStatusesRequest.Response result, GetSellOfferStatusesRequest.RequestParams request)
        {
            var statuses = new List<string>(result.statuses);
            foreach (var item in OrderStatusUtil.GetStatusesInClient())
            {
                if (!statuses.Contains(item.SearchValue))
                {
                    LogWarning(string.Format("{0} with search value {1} not " +
                        "exist in server", item.Title, item.SearchValue));
                }
                else
                {
                    ResultCategories.Add(item);
                    statuses.Remove(item.SearchValue);
                }
            }
            foreach (var item in statuses)
            {
                LogWarning(item + " status not implement in client");
            }
            Terminate(true);
        }

        private void LogWarning(string value)
        {
            DevLogger.Warning(value, MarketLogType.MarketApi);
        }
    }
}