using System;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.UI.Forms
{
    public sealed class RegisterForm : AuthorizationFormBase
    {
        public event Action GoToLoginClicked;

        [SerializeField]
        private Button _toLogInButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            _toLogInButton.onClick.AddListener(OnToLoginClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _toLogInButton.onClick.RemoveListener(OnToLoginClicked);
        }

        private void OnToLoginClicked()
        {
            GoToLoginClicked.SafeRaise();
        }
    }
}