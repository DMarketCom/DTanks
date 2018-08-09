using DMarketSDK.Market.Domain;
using System.Collections.Generic;
using DMarketSDK.Market.Components;

namespace DMarketSDK.Market.Forms
{
    public abstract class TableWithFiltersFormBase<T> : TableFormBase<T> where T : ItemsFormModel
    {
        public bool HaveFilters
        {
            get
            {
                return Container.GetShowingComponent<FilterByContainerComponent>().HaveFilters;
            }
        }

        public bool HaveOrderByFilters
        {
            get
            {
                return Container.GetShowingComponent<OrderByComponentBase>().HaveFilters;
            }
        }

        public void AddFilters(List<FilterCategory> filters)
        {
            var activeFilters = new List<int>();
            for (var i = 0; i < filters.Count; i++)
            {
                activeFilters.Add(i);
            }
            Container.GetShowingComponent<FilterByContainerComponent>().AddFilters(filters, activeFilters);
        }

        public void AddOrderBy(List<FilterCategory> filters)
        {
            Container.GetShowingComponent<OrderByComponentBase>().ApplyOrderFilters(filters);
        }
    }
}
