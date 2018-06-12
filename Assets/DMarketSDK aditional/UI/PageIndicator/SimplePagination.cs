using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Common.UI
{
    public class SimplePagination : PageIndicatorViewBase
    {
        [SerializeField]
        private TextMeshProUGUI _currentPageText;
        [SerializeField]
        private Button _nextPageButton;
        [SerializeField]
        private Button _previousPageButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            _nextPageButton.onClick.AddListener(() => OnNeedChangePage(CurrentPage + 1));
            _previousPageButton.onClick.AddListener(() => OnNeedChangePage(CurrentPage - 1));
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _nextPageButton.onClick.RemoveAllListeners();
            _previousPageButton.onClick.RemoveAllListeners();
        }

        public override void UpdatePage(int currentPage, int totalPages)
        {
            base.UpdatePage(currentPage, totalPages);
            _currentPageText.text = (CurrentPage + 1).ToString();
            _nextPageButton.interactable = CurrentPage < totalPages - 1;
            _previousPageButton.interactable = CurrentPage > 0;
        }
    }
}
