using SHLibrary.Logging;

namespace DMarketSDK.IntegrationAPI.Request
{
    public class Price
    {
        public const string DMC = "DMC";

        public long Amount;
        public string Currency = DMC;

        public Price() { }

        public Price(long amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Amount, Currency);
        }

        public static Price operator -(Price p1, Price p2)
        {
            if (p1.Currency == p2.Currency)
            {
                var result = new Price
                {
                    Currency = p1.Currency,
                    Amount = p1.Amount - p2.Amount
                };
                return result;
            }

            DevLogger.Error("Cannot make operation [-] with different currency types");
            return null;
        }

        public override bool Equals(object obj)
        {
            var castedObj = (Price) obj;
            if (castedObj == null)
            {
                return false;
            }

            return Amount == castedObj.Amount
                   && Currency == castedObj.Currency;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
