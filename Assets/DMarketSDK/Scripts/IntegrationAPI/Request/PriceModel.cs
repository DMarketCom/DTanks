namespace DMarketSDK.IntegrationAPI.Request
{
    public class Price
    {
        public const string DMC = "DMC";

        public int Amount = 0;
        public string Currency = DMC;

        public override string ToString()
        {
            return string.Format("{0} {1}", Amount, Currency);
        }
    }
}
