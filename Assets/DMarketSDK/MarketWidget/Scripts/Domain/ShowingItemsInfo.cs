using System.Collections.Generic;

namespace DMarketSDK.Market.Domain
{
    public struct ShowingItemsInfo
    {
		public string ClassId;
        public int Offset;
        public int Limit;
        public List<string> Categories;
        public string SearchPattern;
        public string OrderBy;
        public OrderDirectionType OrderByDirection;
        public long MinPriceRange;
        public long MaxPriceRange;

        public ShowingItemsInfo(int offset, int limit)
        {
            ClassId = string.Empty;
            Offset = offset;
            Limit = limit;
            Categories = new List<string>();
            SearchPattern = string.Empty;
            OrderBy = string.Empty;
            OrderByDirection = OrderDirectionType.None;
            MinPriceRange = 0;
            MaxPriceRange = 0;
        }

        public bool HaveCategoriesFilter { get { return Categories.Count > 0; } }

        public bool HaveSearchFilter { get { return SearchPattern.Length > 0; } }

        public static bool operator ==(ShowingItemsInfo c1, ShowingItemsInfo c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(ShowingItemsInfo c1, ShowingItemsInfo c2)
        {
            return !c1.Equals(c2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var target = (ShowingItemsInfo)obj;
            return Offset == target.Offset && Limit == target.Limit
                && Categories.Count == target.Categories.Count
                && SearchPattern == target.SearchPattern
                && OrderBy == target.OrderBy
                && OrderByDirection == target.OrderByDirection
                && MinPriceRange == target.MinPriceRange
                && MaxPriceRange == target.MaxPriceRange;
    }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void ChangePriceFilter(long minPrice, long maxPrice)
        {
            MinPriceRange = minPrice;
            MaxPriceRange = maxPrice;
        }

        public string GetCategoriesBody()
        {
            var result = string.Empty;
            for (var i = 0; i < Categories.Count; i++)
            {
                if (i != 0)
                {
                    result += ",";
                }
                result += Categories[i];
            }
            return result;
        }

        public string GetDirByBody()
        {
            switch (OrderByDirection)
            {
                case OrderDirectionType.Asc:
                    return "asc";
                case OrderDirectionType.Desc:
                    return "desc";
                default:
                    return string.Empty;
            }
        }
    }
}