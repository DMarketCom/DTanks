using SHLibrary;
using System;

namespace DMarketSDK.Common.UI
{
    public abstract class PageIndicatorViewBase : UnityBehaviourBase
    {
        public event Action<int> PageChangeClicked;

        public int TotalPages { get; private set; }
        public int CurrentPageIndex { get; private set; }

        protected PageIndicatorViewBase()
        {
            TotalPages = -1;
            CurrentPageIndex = -1;
        }

        public virtual void UpdateWithoutEvent(int currentPage, int totalPages)
        {
            CurrentPageIndex = currentPage;
            TotalPages = totalPages;
        }

        public virtual void UpdatePage(int currentPage, int totalPages)
        {
            if (totalPages != TotalPages)
            {
                TotalPages = totalPages;
                OnTotalPagesChanged();
            }
            if (CurrentPageIndex != currentPage)
            {
                CurrentPageIndex = currentPage;
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