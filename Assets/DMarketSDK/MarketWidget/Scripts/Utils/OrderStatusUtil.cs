using DMarketSDK.Market.Domain;
using System.Collections.Generic;

namespace DMarketSDK
{
    public static class OrderStatusUtil
    {
        private static readonly List<FilterCategory> _filters;

        static OrderStatusUtil()
        {
            _filters = new List<FilterCategory>
            {
                new FilterCategory("Active", "active"),
                new FilterCategory("Pending", "pending"),
                new FilterCategory("Closed", "closed"),
                new FilterCategory("Canceled", "canceled"),
                new FilterCategory("Failed", "failed")
            };
        }

        public static List<FilterCategory> GetStatusesInClient()
        {
            return _filters;
        }
    }
}