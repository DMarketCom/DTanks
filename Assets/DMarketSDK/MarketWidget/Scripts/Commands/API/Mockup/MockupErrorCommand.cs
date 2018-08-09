using DMarketSDK.IntegrationAPI;

namespace DMarketSDK.Market.Commands.API
{
    public class MockupErrorCommand : ApiCommandBase
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
            OnError(new Error());
        }
    }
}