using System;

namespace DMarketSDK.Domain
{
    [Serializable]
    public class LoginEventData
    {
        public readonly string MarketAccessToken;
        public readonly string MarketRefreshAccessToken;
        public readonly string Username;

        public LoginEventData(string marketAccessToken, string marketRefreshAccessToken,
            string username)
        {
            MarketAccessToken = marketAccessToken;
            MarketRefreshAccessToken = marketRefreshAccessToken;
            Username = username;
        }
    }
}