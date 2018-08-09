namespace TankGame.Inventory.Domain
{
    public class DMarketGameTokenResponse
    {
        public string BasicAccessToken;
        public string BasicRefreshToken;
        public string ErrorText;

        public DMarketGameTokenResponse() { }

        public DMarketGameTokenResponse(string basicAccessToken, string basicRefreshToken)
        {
            BasicAccessToken = basicAccessToken;
            BasicRefreshToken = basicRefreshToken;
            ErrorText = string.Empty;
        }
    }
}
