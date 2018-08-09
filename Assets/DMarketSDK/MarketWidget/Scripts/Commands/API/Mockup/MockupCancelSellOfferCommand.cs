namespace DMarketSDK.Market.Commands.API
{
    public sealed class MockupCancelSellOfferCommand : ApiCommandBase
    {
        private const float KSimulationTime = 0.1f;

        public MockupCancelSellOfferCommand(string sellOfferId)
        {

        }

        public override void Start()
        {
            base.Start();
            ScheduledUpdate(KSimulationTime);
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();

            Terminate(true);
        }
    }
}