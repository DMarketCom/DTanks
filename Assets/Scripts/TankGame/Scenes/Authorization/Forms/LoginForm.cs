using System;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.UI.Forms
{
    public sealed class LoginForm : AuthorizationFormBase
    {
        public event Action SignUpClicked;
        public event Action BackClicked; 

        [SerializeField]
        private Button _signUpButton;
        [SerializeField]
        private Button _backButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            _signUpButton.onClick.AddListener(OnSignUpClicked);
            _backButton.onClick.AddListener(OnBackClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _signUpButton.onClick.RemoveListener(OnSignUpClicked);
            _backButton.onClick.RemoveListener(OnBackClicked);
        }

        private void OnSignUpClicked()
        {
            SignUpClicked.SafeRaise();
        }

        private void OnBackClicked()
        {
            BackClicked.SafeRaise();
        }
    }
}