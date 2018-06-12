using SHLibrary;
using SHLibrary.Utils;
using System;

namespace DMarketSDK.Common.UI
{
    public abstract class PageIndicatorViewBase : UnityBehaviourBase
    {
        public event Action<int> PageChangeClicked;

        public int TotalPages { get; private set; }
        public int CurrentPage { get; private set; }

        protected PageIndicatorViewBase()
        {
            TotalPages = -1;
            CurrentPage = -1;
        }

        public virtual void UpdatePage(int currentPage, int totalPages)
        {
            if (totalPages != TotalPages)
            {
                TotalPages = totalPages;
                OnTotalPagesChanged();
            }
            if (CurrentPage != currentPage)
            {
                CurrentPage = currentPage;
                OnCurrentPageChanged();
            }
        }

        protected virtual void OnTotalPagesChanged()
        { }

        protected virtual void OnCurrentPageChanged()
        { }

        protected void OnNeedChangePage(int pageValue)
        {
            PageChangeClicked.SafeRaise(pageValue);
        }
    }
}