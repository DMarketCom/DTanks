using DMarketSDK.Market.Domain;
using SHLibrary.Logging;
using System.Collections.Generic;
using UnityEngine;

namespace DMarketSDK.Market.Components
{
    public abstract class OrderByComponentBase : ShowingContainerComponentBase
    {
        [SerializeField]
        private List<FilterCategory> _filtersInClient;

        public bool HaveFilters { get { return Filters != null; } }

        protected List<FilterCategory> Filters { get; private set; }

        public void ApplyOrderFilters(List<FilterCategory> filters)
        {
            var filtersQuery = new List<string>();
            foreach (var item in filters)
            {
                filtersQuery.Add(item.SearchValue);
            }
            MergeFilters(filtersQuery);
            OnStart();
        }

        protected abstract void OnStart();

        private void MergeFilters(List<string> filtersInServer)
        {
            Filters = new List<FilterCategory>();
            foreach (var item in _filtersInClient)
            {
                if (!filtersInServer.Contains(item.SearchValue))
                {
                    LogWarning(string.Format("{0} with search value {1} not " +
                        "exist in server", item.Title, item.SearchValue));
                }
                else
                {
                    Filters.Add(item);
                }
            }
            foreach (var item in filtersInServer)
            {
                var filtrInClient = Filters.Find(filtr => filtr.SearchValue == item);
                if (filtrInClient == null)
                {
                    LogWarning(string.Format("{0} have no client implementation", item));
                }
            }
        }

        protected void LogWarning(string value)
        {
            DevLogger.Warning(value, MarketLogType.MarketApi);
        }
    }
}