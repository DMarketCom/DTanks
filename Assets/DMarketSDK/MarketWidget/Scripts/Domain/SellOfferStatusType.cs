using System.Collections.Generic;

namespace DMarketSDK.Market.Domain
{
    public enum SellOfferStatusType
    {
        None = 0,
        Unknow,
        Active,
        InProgress,
        Closed,
        Canceled,
        Failed
    }

    public static class SellOfferStatusTypeExtentions
    {
        private readonly static Dictionary<string, SellOfferStatusType> _mapStatuses;

        static SellOfferStatusTypeExtentions()
        {
            _mapStatuses = new Dictionary<string, SellOfferStatusType>();
            _mapStatuses.Add("active", SellOfferStatusType.Active);
            _mapStatuses.Add("pending", SellOfferStatusType.InProgress);
            _mapStatuses.Add("closed", SellOfferStatusType.Closed);
            _mapStatuses.Add("canceled", SellOfferStatusType.Canceled);
            _mapStatuses.Add("failed", SellOfferStatusType.Failed);
        }

        public static SellOfferStatusType GetSellOfferStatusType(string status)
        {
            if (_mapStatuses.ContainsKey(status))
            {
                return _mapStatuses[status];
            }
            else
            {
                return SellOfferStatusType.Unknow;
            }
        }

        public static string GetText(this SellOfferStatusType status)
        {
            foreach (var key in _mapStatuses.Keys)
            {
                if (_mapStatuses[key] == status)
                {
                    return key;
                }
            }
            return status.ToString();
        }

    }
}