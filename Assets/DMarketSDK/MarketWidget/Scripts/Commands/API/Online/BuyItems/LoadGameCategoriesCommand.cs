using DMarketSDK.IntegrationAPI.Request.MarketIntegration;
using DMarketSDK.Market.Domain;

namespace DMarketSDK.Market.Commands.API
{
    public class LoadGameCategoriesCommand : LoadFiltersCommandBase
    {   
        public override void Start()
        {
            base.Start();
            MarketApi.GetGameCategories(OnSuccesCallback, OnError);
        }

        private void OnSuccesCallback(GetGameCategoriesRequest.Response responce, GetGameCategoriesRequest.RequestParams request)
        {
            var categories = responce.Categories;
            for (var i = 0; i < categories.Count; i++)
            {
                var currentCategory = categories[i];
                ResultCategories.Add(new FilterCategory(currentCategory.title, currentCategory.value));
            }
            Terminate(true);
        }
    }
}