using DMarketSDK.Market.Domain;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DMarketSDK.Market.Commands.API
{
    public abstract class LoadFiltersCommandBase : ApiCommandBase
    {
        public List<FilterCategory> ResultCategories { get; private set; }

        public override void Start()
        {
            base.Start();
            ResultCategories = new List<FilterCategory>();
        }

        protected string GetFilterTitle(string value)
        {
            return Regex.Replace(value, "[A-Z]", " $0").Trim();
        }
    }
}
