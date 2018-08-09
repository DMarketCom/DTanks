using System;

namespace DMarketSDK.Market
{
    public interface IMarketHeader
    {
        event Action ClosedClicked;
        event Action LogoutClicked;

        void SetActiveHeaderPanel(bool isActive);

        void SetActiveLoggedHeaderPanel(bool isActive);
    }
}