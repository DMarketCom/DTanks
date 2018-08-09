using DMarketSDK.Market.Items;

namespace DMarketSDK.Market.Commands.API
{
    public abstract class ApiItemOperationCommandBase : ApiCommandBase
    {
        public readonly MarketItemModel TargetItemModel;

        public override bool IsIndependentFromState
        {
            get { return true; }
        }

        protected ApiItemOperationCommandBase(MarketItemModel targetItemModel)
        {
            TargetItemModel = targetItemModel;
        }
    }
}
