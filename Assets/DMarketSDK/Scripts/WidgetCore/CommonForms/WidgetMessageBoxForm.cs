using System;
using DMarketSDK.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.WidgetCore.Forms
{
    public sealed class WidgetMessageBoxFormModel : WidgetFormModel
    {

    }

    public class WidgetMessageBoxForm : WidgetFormViewBase<WidgetMessageBoxFormModel>
    {
        public event Action Closed;
        
        [SerializeField]
        private TextMeshProUGUI _messageText;

        [SerializeField]
        private TextMeshProUGUI _titleText;

        [SerializeField]
        private TextMeshProUGUI _closeButtonText;

        [SerializeField]
        private Button _closeButton;

        public void Show(string titleText, string messageText, string closeButtonText = "OK")
        {
            base.Show();

            _messageText.text = messageText;
            _titleText.text = titleText;
            _closeButtonText.text = closeButtonText;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _closeButton.onClick.AddListener(OnCloseClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _closeButton.onClick.RemoveListener(OnCloseClicked);
        }

        private void OnCloseClicked()
        {
            Closed.SafeRaise();
            Hide();
        }
    }
}