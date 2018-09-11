using DMarketSDK.Domain;
using DMarketSDK.IntegrationAPI.Request;
using DMarketSDK.Market.Items;

namespace DMarketSDK.Forms
{
    public sealed class WidgetModel : WidgetFormModel
    {
        public string BasicAccessToken   { get; private set; }
        public string BasicRefreshToken  { get; private set; }

        public string MarketAccessToken  { get; private set; }
        public string MarketRefreshToken { get; private set; }

        public string UserName;
        public Price Balance = new Price();

        public ScreenOrientationSettings GameOrientationSettings { get; private set; }

        private ItemModelType _currentItemType = ItemModelType.None;

        public ItemModelType CurrentItemType
        {
            get { return _currentItemType; }
            set { _currentItemType = value; }
        }

        public void SetBasicTokens(string basicAccessToken, string basicRefreshToken)
        {
            BasicAccessToken = basicAccessToken;
            BasicRefreshToken = basicRefreshToken;
        }

        public void SetMarketTokens(string marketAccessToken, string marketRefreshToken)
        {
            MarketAccessToken = marketAccessToken;
            MarketRefreshToken = marketRefreshToken;
        }

        public void SetGameScreenSettings(ScreenOrientationSettings settings)
        {
            GameOrientationSettings = settings;
        }

        public void ClearUserData()
        {
            UserName = string.Empty;
            Balance = new Price();
            _currentItemType = ItemModelType.None;
        }
    }
}