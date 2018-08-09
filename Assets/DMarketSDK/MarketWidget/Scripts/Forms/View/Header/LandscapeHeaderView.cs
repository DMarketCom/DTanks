using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Market
{
    public sealed class LandscapeHeaderView : MarketHeaderView
    {
        [SerializeField]
        private TextMeshProUGUI _txtUserName;
        [SerializeField]
        private TextMeshProUGUI _txtBalance;

        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private Button _logoutButton;

        [SerializeField]
        private GameObject _authorizedPanel;

        protected override void OnModelChanged()
        {
            _txtUserName.text = Model.UserName;
            _txtBalance.text = Model.Balance.GetStringWithCurrencySprite(_txtBalance.color); ;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _logoutButton.onClick.AddListener(OnLogoutClicked);
            _closeButton.onClick.AddListener(OnCloseClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _logoutButton.onClick.RemoveListener(OnLogoutClicked);
            _closeButton.onClick.RemoveListener(OnCloseClicked);
        }

        public override void SetActiveLoggedHeaderPanel(bool isActive)
        {
            _authorizedPanel.SetActive(isActive);
        }
    }
}