using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.UI.Forms
{
    public class SimpleMessageForm : FormBase
    {
        public event Action Closed;

        [SerializeField]
        private TextMeshProUGUI _messageText;

        [SerializeField]
        private Button _closeButton;

        public void Init(string messageText)
        {
            _messageText.text = messageText;
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