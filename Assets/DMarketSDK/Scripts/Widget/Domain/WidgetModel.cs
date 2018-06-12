using SHLibrary.ObserverView;
using System;

namespace DMarketSDK.Widget
{
    public class WidgetModel : ObservableBase
    {
        public string BasicAccessToken { get; private set; }
        public string BasicRefreshToken { get; private set; }

        public string MarketAccessToken;
        public string MarketRefreshAccessToken;
        public string LoggedUsername;
        
        public bool IsLogin { get { return !string.IsNullOrEmpty(LoggedUsername); } }

        public bool IsHaveBasicAcessToken
        {
            get { return !string.IsNullOrEmpty(BasicAccessToken); }
        }

        public bool IsHaveMarketAcessToken
        {
            get { return !string.IsNullOrEmpty(MarketAccessToken); }
        }
        
        public void ApplyTokens(string basicAccessToken, string refreshToken)
        {
            BasicAccessToken = basicAccessToken;
            BasicRefreshToken = refreshToken;
        }

        public void SetLoggedUserData(WidgetUserDataModel model)
        {
            if (model == null) {
                throw new ArgumentNullException("UserDataModel can't be null");
            }
            if (!string.IsNullOrEmpty(model.MarketAccessToken)
                && !string.IsNullOrEmpty(model.LoggedUsername))
            {
                MarketAccessToken = model.MarketAccessToken;
                LoggedUsername = model.LoggedUsername;
            }
        }
    }
}