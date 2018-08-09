using UnityEngine;
using DMarketSDK.Market.Domain;
using TMPro;

namespace DMarketSDK.Market.Containers.Components
{
    public class SearchContainerComponent : ShowingContainerComponentBase
    {
        [SerializeField]
        private TMP_InputField _searchInputField;

        private string _lastSearchInput = string.Empty;

        protected TMP_InputField SearchInputField { get { return _searchInputField; } }

        protected override void OnEnable()
        {
            base.OnEnable();
            _searchInputField.onEndEdit.AddListener(ApplySearchInput);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _searchInputField.onEndEdit.RemoveListener(ApplySearchInput);
        }

        protected void ApplySearchInput(string searchInput)
        {
            _lastSearchInput = searchInput;
            _searchInputField.text = searchInput;

            ApplyChanging();
        }

        #region ShowingContainerComponentBase implemenation

        public override void ResetShowingParams(ShowingItemsInfo showingInfo)
        {
            _searchInputField.text = showingInfo.SearchPattern;
            _lastSearchInput = _searchInputField.text;
        }

        public override void ModifyShowingParams(ref ShowingItemsInfo showingInfo)
        {
            showingInfo.SearchPattern = _lastSearchInput;
        }

        #endregion
    }
}