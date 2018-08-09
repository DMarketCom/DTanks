using TankGame.UI.Forms;
using TMPro;
using UnityEngine;

namespace TankGame.GameClient.UI
{
    public sealed class SimpleMessagePopUp : FormBase
    {
        [SerializeField]
        private TextMeshProUGUI _txtMessage;

        private float _fadeAnimTime;

        protected override float FadeAnimTime
        {
            get
            {
                return _fadeAnimTime;
            }
        }

        public void Init(string messageText, float fadeTime = 0.7f)
        {
            _txtMessage.text = messageText;
            _fadeAnimTime = fadeTime;
        }

        public override void Hide()
        {
            base.Hide();
            _txtMessage.text = string.Empty;
        }
    }
}
