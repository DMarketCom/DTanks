using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Market.Containers.Components
{
    public sealed class StandaloneSearchComponent : SearchContainerComponent
    {
        [SerializeField]
        private Button _clearSearchButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            _clearSearchButton.onClick.AddListener(OnClearSearchClicked);
            SearchInputField.onValueChanged.AddListener(OnSearchValueChanged);

            OnSearchValueChanged(SearchInputField.text);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _clearSearchButton.onClick.RemoveListener(OnClearSearchClicked);
            SearchInputField.onValueChanged.RemoveListener(OnSearchValueChanged);
        }

        private void OnSearchValueChanged(string searchInput)
        {
            _clearSearchButton.gameObject.SetActive(!string.IsNullOrEmpty(searchInput));
        }

        private void OnClearSearchClicked()
        {
            SearchInputField.text = string.Empty;
            ApplySearchInput(string.Empty);
        }
    }
}