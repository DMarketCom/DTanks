using System;

namespace TankGame.Inventory.Domain
{
    public class DMarketGameTokenRequest
    {
        public Action<DMarketGameTokenResponse> Callback;

        public DMarketGameTokenRequest(Action<DMarketGameTokenResponse> callback)
        {
            Callback = callback;
        }
    }
}