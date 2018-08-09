using SHLibrary;
using System;

namespace DMarketSDK.Common.UI
{
    public abstract class TabViewBase : UnityBehaviourBase
    {
        public Action<TabViewBase> Clicked;

        public abstract bool IsSelected { get; }

        public abstract void SetState(bool isSelected, bool useAnimation);

        public abstract bool Interactable { set; }

        public abstract string Title { set; }
    }
}