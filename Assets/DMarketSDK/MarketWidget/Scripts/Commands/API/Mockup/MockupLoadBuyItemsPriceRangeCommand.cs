using DMarketSDK.Market.Domain;

namespace DMarketSDK.Market.Commands.API
{
    public sealed class MockupLoadBuyItemsPriceRangeCommand : ApiCommandBase, ILoadPriceRangeCommand
    {
        private const long MinPriceRange = 0;
        private const long MaxPriceRange = 1000;

        public PriceRangeCommandResult PriceRangeResult { get; private set; }

        public MockupLoadBuyItemsPriceRangeCommand(ShowingItemsInfo showingItemsInfo)
        {

        }

        public override void Start()
        {
            base.Start();

            PriceRangeResult = new PriceRangeCommandResult(MinPriceRange, MaxPriceRange);
            Terminate(true);
        }
    }
}
