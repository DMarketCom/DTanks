using TMPro;
using UnityEngine;

namespace TankGame.UI.Forms
{
    public class NotificationMessageForm : FormBase
    {
        [SerializeField]
        private TextMeshProUGUI _txtTitle;

        private float _fadeAnimTime;

        protected override float FadeAnimTime
        {
            get
            {
                return _fadeAnimTime;
            }
        }

        public void ShowWithMessage(string message, float animTime = 0.5f)
        {
            _fadeAnimTime = animTime;
            Show();
            _txtTitle.text = message;
        }

        public override void Hide()
        {
            base.Hide();
            _txtTitle.text = string.Empty;
        }
    }
}