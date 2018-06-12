namespace DMarketSDK.Widget
{
    public class WidgetUserDataModel
    {
        public string MarketAccessToken;
        public string LoggedUsername;

        public WidgetUserDataModel(string marketAccessToken, string loggedUsername)
        {
            MarketAccessToken = marketAccessToken;
            LoggedUsername = loggedUsername;
        }
    }
}
