using DMarketSDK.Common.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.Inventory
{
    public sealed class InventoryPagination : PageIndicatorViewBase
    {
        [SerializeField]
        private TextMeshProUGUI _pageText;
        [SerializeField]
        private Button _nextPageButton;
        [SerializeField]
        private Button _previousPageButton;

        protected override void OnEnable()
        {
            base.OnEnable();

            _nextPageButton.onClick.AddListener(OnNextPageClicked);
            _previousPageButton.onClick.AddListener(OnPreviousPageClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _nextPageButton.onClick.RemoveListener(OnNextPageClicked);
            _previousPageButton.onClick.RemoveListener(OnPreviousPageClicked);
        }

        public override void UpdatePage(int currentPage, int totalPages)
        {
            base.UpdatePage(currentPage, totalPages);

            _pageText.text = string.Format("{0}/{1}", CurrentPageIndex + 1, TotalPages);
            _nextPageButton.interactable = CurrentPageIndex < TotalPages - 1;
            _previousPageButton.interactable = CurrentPageIndex > 0;
        }

        private void OnNextPageClicked()
        {
            OnNeedChangePage(CurrentPageIndex + 1);
        }

        private void OnPreviousPageClicked()
        {
            OnNeedChangePage(CurrentPageIndex - 1);
        }
    }
}
