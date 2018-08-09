using DMarketSDK.Market.Domain;

namespace DMarketSDK.Market.Commands.API
{
    public class MockupLoadOrderByListCommand : LoadFiltersCommandBase
    {
        private const float kSimulationTime = 0.7f;

        public override void Start()
        {
            base.Start();
            ScheduledUpdate(kSimulationTime, false);
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();
            ResultCategories.Add(new FilterCategory(string.Empty, "title"));
            ResultCategories.Add(new FilterCategory(string.Empty, "price"));
            ResultCategories.Add(new FilterCategory(string.Empty, "fee"));
            ResultCategories.Add(new FilterCategory(string.Empty, "status"));
            ResultCategories.Add(new FilterCategory(string.Empty, "created"));
            Terminate(true);
        }
    }
}