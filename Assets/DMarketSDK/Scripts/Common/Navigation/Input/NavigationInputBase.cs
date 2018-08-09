using System;
using SHLibrary;

namespace DMarketSDK.Common.Navigation
{
    public abstract class NavigationInputBase : UnityBehaviourBase
    {
        public static event Action<NavigationType> Clicked;

        protected void SendNavigationEvent(NavigationType navigationType)
        {
            Clicked.SafeRaise(navigationType);
        }
    }
}