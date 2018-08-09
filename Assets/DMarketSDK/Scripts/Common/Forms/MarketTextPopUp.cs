using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Common.Forms
{
    public class MarketTextPopUp : AnimMarketFormBase
    {
        public event Action Closed;

        [SerializeField]
        private List<Button> _closeButtons;
        [SerializeField]
        private TextMeshProUGUI _messageText;

        private float _closeTime = -1;

        public void Show(string message, float closeAfterTime = 10f)
        {
            _messageText.text = message;
            _closeButtons.ForEach(button => button.onClick.AddListener(OnCloseClicked));
            _closeTime = closeAfterTime + Time.timeSinceLevelLoad;
            base.Show();
        }

        private void Update()
        {
            if (_closeTime > 0 && _closeTime < Time.timeSinceLevelLoad)
            {
                _closeTime = -1;
                Hide();
            }
        }
        
        private void OnCloseClicked()
        {
            Hide();
        }

        public override void Hide()
        {
            base.Hide();
            _closeButtons.ForEach(button => button.onClick.RemoveAllListeners());
            Closed.SafeRaise();
        }
    }
}