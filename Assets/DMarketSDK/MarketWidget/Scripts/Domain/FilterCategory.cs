using System;

namespace DMarketSDK.Market.Domain
{
    [Serializable]
    public class FilterCategory
    {
        public string Title;
        public string SearchValue;

        public FilterCategory(string title) : this(title, title) { }

        public FilterCategory(string title, string searchValue)
        {
            Title = title;
            SearchValue = searchValue;
        }

        public override string ToString()
        {
            return string.Format("Title: {0}, SearchValue: {1}", Title, SearchValue);
        }
    }
}