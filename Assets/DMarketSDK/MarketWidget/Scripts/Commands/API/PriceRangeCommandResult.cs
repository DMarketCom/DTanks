namespace DMarketSDK.Market.Commands.API
{
    public sealed class PriceRangeCommandResult
    {
        public readonly long MinPriceRange;
        public readonly long MaxPriceRange;

        public PriceRangeCommandResult(long minPriceRange, long maxPriceRange)
        {
            MinPriceRange = minPriceRange;
            MaxPriceRange = maxPriceRange;
        }
    }
}