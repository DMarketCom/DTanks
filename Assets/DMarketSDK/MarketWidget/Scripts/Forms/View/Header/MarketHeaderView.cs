using System;
using DMarketSDK.Forms;
using SHLibrary.ObserverView;

namespace DMarketSDK.Market
{
    public abstract class MarketHeaderView : ObserverViewBase<WidgetModel>, IMarketHeader
    {
        public event Action ClosedClicked;
        public event Action LogoutClicked;

        public abstract void SetActiveLoggedHeaderPanel(bool isActive);

        public void SetActiveHeaderPanel(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        protected void OnCloseClicked()
        {
            ClosedClicked.SafeRaise();
        }

        protected void OnLogoutClicked()
        {
            LogoutClicked.SafeRaise();
        }
    }
}