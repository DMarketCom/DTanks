using SHLibrary.Logging;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Market
{
    public sealed class PortraitHeaderView : MarketHeaderView
    {
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private Button _menuButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            _closeButton.onClick.AddListener(OnCloseClicked);
            _menuButton.onClick.AddListener(OnMenuClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _closeButton.onClick.RemoveListener(OnCloseClicked);
            _menuButton.onClick.RemoveListener(OnMenuClicked);
        }

        protected override void OnModelChanged()
        {

        }

        public override void SetActiveLoggedHeaderPanel(bool isActive)
        {
            _menuButton.gameObject.SetActive(isActive);
        }

        private void OnMenuClicked()
        {
            DevLogger.Error("Need implement menu page.");
        }
    }
}