namespace DMarketSDK.Market.Commands.API
{
    public class MockupLoadUserBalance : ApiCommandBase
    {
        public override void Start()
        {
            base.Start();
            ScheduledUpdate(0.2f);
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();
            MarketModel.Balance.Amount = 1000;
            MarketModel.SetChanges();
            Terminate(true);
        }
    }
}