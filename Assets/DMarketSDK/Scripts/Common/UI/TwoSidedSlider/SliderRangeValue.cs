namespace DMarketSDK.Common.UI
{
    public struct SliderRangeValue
    {
        public readonly float MinValue;
        public readonly float MaxValue;

        public SliderRangeValue(float minValue, float maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}