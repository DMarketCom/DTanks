using System;
using DMarketSDK.IntegrationAPI;
using TankGame.DMarketIntegration;

namespace TankGame.GameServer.Commands.DMarket
{
    public abstract class ServerDMarketCommandBase : ServerAppCommandBase
    {
        protected ServerApi DMarketApi { get { return Controller.DMarketServerApi; } }

        protected readonly DMarketInfoConverter DMarketConverter = new DMarketInfoConverter();

        protected void RefreshMarketToken(int connectionId, Action successCallback, Action<ErrorCode> failureCallback)
        {
            string userRefreshToken = Model.GetPlayerMarketRefreshToken(connectionId);
            DMarketApi.GetMarketRefreshToken(userRefreshToken,
                (response, requestParam) =>
                {
                    Model.SetPlayerMarketAccessToken(connectionId, response.token);
                    Model.SetPlayerMarketRefreshToken(connectionId, response.refreshToken);
                    Model.SetChanges();
                    successCallback.SafeRaise();
                },
                error =>
                {
                    failureCallback.SafeRaise(error.ErrorCode);
                });
        } 
    }
}