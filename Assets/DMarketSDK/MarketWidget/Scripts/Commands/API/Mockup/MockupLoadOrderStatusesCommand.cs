namespace DMarketSDK.Market.Commands.API
{
    public class MockupLoadOrderStatusesCommand : LoadFiltersCommandBase
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
            ResultCategories.AddRange(OrderStatusUtil.GetStatusesInClient().ToArray());
            Terminate(true);
        }
    }
}