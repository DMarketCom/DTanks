using System;
using DMarketSDK.Common.UI;
using DMarketSDK.WidgetCore.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Forms
{
    public sealed class WidgetLoginForm : WidgetFormViewBase<WidgetLoginFormModel>
    {
        public event Action LoginClicked;
        public event Action SignUpClicked;
        public event Action SignUpStepTwoClicked;
        public event Action ForgotPasswordClicked;
        public event Action<string> LoginEndEdit;
        public event Action<string> PasswordEndEdit;
        public event Action CloseClicked;

        [SerializeField]
        private Button _signUpButton;
        [SerializeField]
        private Button _signUpButtonStepTwo;
        [SerializeField]
        private Button _forgotPasswordButton;

        [SerializeField]
        private TMP_InputField _loginField;
        [SerializeField]
        private TMP_InputField _passwordField;
        [SerializeField]
        private Button _loginButton;
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private GameObject _errorPanel;
        [SerializeField]
        private TextMeshProUGUI _errorText;
        [SerializeField]
        private LoadingSpinner _loadingSpinner;

        protected override void OnEnable()
        {
            base.OnEnable();

            _loginField.onEndEdit.AddListener(OnLoginEndEdit);
            _passwordField.onEndEdit.AddListener(OnPasswordEndEdit);

            _loginButton.onClick.AddListener(OnLoginClicked);
            _signUpButton.onClick.AddListener(OnSignUpClicked);
            _signUpButtonStepTwo.onClick.AddListener(OnSignUpStepTwoClicked);
            _forgotPasswordButton.onClick.AddListener(OnForgotPasswordClicked);
            _closeButton.onClick.AddListener(OnCloseClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _loginField.onEndEdit.RemoveListener(OnLoginEndEdit);
            _passwordField.onEndEdit.RemoveListener(OnPasswordEndEdit);

            _loginButton.onClick.RemoveListener(OnLoginClicked);
            _signUpButton.onClick.RemoveListener(OnSignUpClicked);
            _signUpButtonStepTwo.onClick.RemoveListener(OnSignUpStepTwoClicked);
            _forgotPasswordButton.onClick.RemoveListener(OnForgotPasswordClicked);
            _closeButton.onClick.RemoveListener(OnCloseClicked);
        }

        public override void Show()
        {
            base.Show();
            SetWaitState(false);
            HideError();
        }

        protected override void OnModelChanged()
        {
            _loginField.text = FormModel.UserLogin;
            _passwordField.text = FormModel.UserPassword;
        }

        public void ShowError(string errorMessage)
        {
            _errorText.text = errorMessage;
            _errorPanel.gameObject.SetActive(true);
        }

        public void HideError()
        {
            _errorText.text = string.Empty;
            _errorPanel.SetActive(false);
        }

        public void SetWaitState(bool wait)
        {
            _loginButton.interactable = !wait;

            _loadingSpinner.SetActiveSpinner(wait);
        }

        public void ClearFields()
        {
            _loginField.text = string.Empty;
            _passwordField.text = string.Empty;
            _errorText.text = string.Empty;
        }

        private void OnSignUpClicked()
        {
            SignUpClicked.SafeRaise();
        }

        private void OnSignUpStepTwoClicked()
        {
            SignUpStepTwoClicked.SafeRaise();
        }

        private void OnForgotPasswordClicked()
        {
            ForgotPasswordClicked.SafeRaise();
        }

        private void OnLoginClicked()
        {
            LoginClicked.SafeRaise();
        }

        private void OnPasswordEndEdit(string userPassword)
        {
            PasswordEndEdit.SafeRaise(userPassword);
        }

        private void OnLoginEndEdit(string userLogin)
        {
            LoginEndEdit.SafeRaise(userLogin);
        }

        private void OnCloseClicked()
        {
            CloseClicked.SafeRaise();
        }
    }
}