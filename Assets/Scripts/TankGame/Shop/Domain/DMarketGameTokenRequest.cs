using System;

namespace Shop.Domain
{
    public class DMarketGameTokenRequest
    {
        public Action<DMarketGameTokenResponce> Callback;

        public DMarketGameTokenRequest(Action<DMarketGameTokenResponce> callback)
        {
            Callback = callback;
        }
    }
}