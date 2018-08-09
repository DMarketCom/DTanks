using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.WidgetCore.Forms
{
    public class ApproveForm : WidgetFormViewBase
    {
        private event Action<bool> Result;

        [SerializeField]
        private TextMeshProUGUI _messageText;

        [SerializeField]
        private Button _yesButton;

        [SerializeField]
        private Button _noButton;

        public void Show(string messageText)
        {
            base.Show();

            _messageText.text = messageText;
        }

        public void ShowChoiceWindow(string messageText, Action<bool> result)
        {
            Result = result;
            Show(messageText);
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

        private void OnYesClicked()
        {
            Result.SafeRaise(true);
            Hide();
        }

        private void OnNoClicked()
        {
            Result.SafeRaise(false);
            Hide();
        }
    }
}