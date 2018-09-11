using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.WidgetCore.Forms
{
    public class ApproveForm : WidgetFormViewBase
    {
        private Action<bool> _result;

        [SerializeField]
        private TextMeshProUGUI _messageText;

        [SerializeField]
        private Button _yesButton;

        [SerializeField]
        private Button _noButton;

        public void ShowChoiceWindow(string messageText, Action<bool> result)
        {
            _result = result;
            _messageText.text = messageText;
            Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _yesButton.onClick.AddListener(OnYesClicked);
            _noButton.onClick.AddListener(OnNoClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _yesButton.onClick.RemoveListener(OnYesClicked);
            _noButton.onClick.RemoveListener(OnNoClicked);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _result = null;
        }

        private void OnYesClicked()
        {
            _result.SafeRaise(true);
            Hide();
        }

        private void OnNoClicked()
        {
            _result.SafeRaise(false);
            Hide();
        }
    }
}