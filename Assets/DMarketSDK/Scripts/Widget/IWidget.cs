using DMarketSDK.IntegrationAPI;
using System;

namespace DMarketSDK.Widget
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

    public interface IWidget
    {
        event Action<LoginEventData> LoginEvent;
        event Action LogoutEvent;
        event Action ClosedEvent;
        event Action<Error> ErrorEvent;

        string MarketAccessToken { get; }
        string LoggedUserName { get; }
        bool IsInitialize { get; }
        bool IsLogged { get; }

        void Init(ClientApi clientApi, string gameToken, string refreshToken);
        void Open();
        void Close();
        void Logout();
        void SetLoggedUserData(WidgetUserDataModel model);
    }
}