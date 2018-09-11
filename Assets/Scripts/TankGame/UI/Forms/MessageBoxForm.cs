using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.UI.Forms
{
    public sealed class MessageBoxForm : FormBase
    {
        public event Action Closed;

        [SerializeField]
        private TextMeshProUGUI _txtTitle;

        [SerializeField]
        private TextMeshProUGUI _txtMessage;

        [SerializeField]
        private Button _btnOk;

        [SerializeField]
        private Button _btnClose;

        public void Show(string titleText, string messageText)
        {
            base.Show();

            _txtTitle.text = titleText;
            _txtMessage.text = messageText;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _btnClose.onClick.AddListener(OnCloseClicked);
            _btnOk.onClick.AddListener(OnCloseClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _btnClose.onClick.RemoveListener(OnCloseClicked);
            _btnOk.onClick.RemoveListener(OnCloseClicked);
        }

        private void OnCloseClicked()
        {
            Closed.SafeRaise();
            Hide();
        }
    }
}