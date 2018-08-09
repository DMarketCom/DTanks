using DMarketSDK.Market.Items;
using Random = UnityEngine.Random;

namespace DMarketSDK.Market.Commands.API
{
    public class MockupItemOperationCommand : ApiItemOperationCommandBase
    {
        private const float KMinLoadingTime = 0.1f;
        private const float KMaxLoadingTime = 2f;

        public MockupItemOperationCommand(MarketItemModel targetItemModel) : base(targetItemModel)
        {
        }

        public override void Start()
        {
            base.Start();
            var loadingTime = Random.Range(KMinLoadingTime, KMaxLoadingTime);
            ScheduledUpdate(loadingTime);
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();
            if (Random.value > 0.5f)
            {
                Terminate(true);
            }
            else
            {
                OnError("Mockup model error");
            }
        }
    }
}