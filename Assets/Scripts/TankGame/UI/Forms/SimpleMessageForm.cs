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

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseClicked);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseClicked);
        }

        private void OnCloseClicked()
        {
            Closed.SafeRaise();
            Hide();
        }
    }
}