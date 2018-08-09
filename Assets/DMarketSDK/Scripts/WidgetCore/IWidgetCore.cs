using DMarketSDK.IntegrationAPI;
using System;
using DMarketSDK.Domain;
using DMarketSDK.Market;

namespace DMarketSDK.Basic
{
    public interface IWidgetCore
    {
        event Action<LoginEventData> LoginEvent;
        event Action LogoutEvent;
        event Action<Error> ErrorEvent;

        event Action ClosedEvent;

        bool IsLogged { get; }
        bool IsInitialized { get; }

        void Init(string basicAccessToken, string basicRefreshToken, ClientApi clientApi);

        void Close();

        void Logout();

        void Dispose();

        string GetErrorMessage(ErrorCode errorCode);
    }
}