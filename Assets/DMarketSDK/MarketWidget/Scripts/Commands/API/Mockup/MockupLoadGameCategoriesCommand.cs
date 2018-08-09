using DMarketSDK.Market.Domain;
using TankGame.Domain.GameItem;

namespace DMarketSDK.Market.Commands.API
{
    public class MockupLoadGameCategoriesCommand : LoadFiltersCommandBase
    {
        private const float kLoadingTime = 0.1f;

        public override void Start()
        {
            base.Start();
            ScheduledUpdate(kLoadingTime, false);
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();
            ResultCategories.Add(new FilterCategory("Skins", GameItemCategory.Skin.ToString()));
            ResultCategories.Add(new FilterCategory("Helmets", GameItemCategory.Helmet.ToString()));
            Terminate(true);
        }
    }
}