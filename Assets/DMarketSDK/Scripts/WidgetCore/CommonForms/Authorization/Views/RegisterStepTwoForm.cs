using System;
using DMarketSDK.Common.UI;
using DMarketSDK.Domain;
using DMarketSDK.WidgetCore.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Forms
{
    public class RegisterStepTwoForm : WidgetFormViewBase<RegistrationFormModel>
    {
        public event Action<string> OnEmailEntered;
        public event Action<bool> OnTermsOfUseToggleChecked;
        public event Action<string> OnReEnterPasswordEntered;
        public event Action<string> OnPasswordEntered;
        public event Action<string> OnAccountNameEntered;
        public event Action CloseClicked;
        public event Action BackClicked;
        public event Action CreateAccountClicked;

        public event Action TermOfUseClicked;
        public event Action PrivacyPolicyClicked;
        public event Action SignInClicked;

        [SerializeField]
        private TMP_InputField _emailField;

        [SerializeField]
        private TMP_InputField _accountName;

        [SerializeField]
        private TMP_InputField _passwordField;

        [SerializeField]
        private TMP_InputField _reEnterPassword;

        [SerializeField]
        private GameObject _errorBlock;

        [SerializeField]
        private TextMeshProUGUI _errorText;

        [SerializeField]
        private Button _createAccountButton;

        [SerializeField]
        private Button _stepOneButton;

        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private LoadingSpinner LoadingSpinner;

        [SerializeField]
        private Toggle _termsOfUseToggle;

        [SerializeField]
        private Button _termOfUseButton;

        [SerializeField]
        private Button _privacyPolicyButton;

        [SerializeField]
        private Button _signInButton;

        protected override void OnModelChanged()
        {
            _emailField.text = FormModel.RegistrationData.Email;
            _accountName.text = FormModel.RegistrationData.AccountName;
            _passwordField.text = FormModel.RegistrationData.Password;
            _reEnterPassword.text = FormModel.RegistrationData.ReEnterPassword;
            _termsOfUseToggle.isOn = FormModel.RegistrationData.TermsOfUseIsEnabled;

            _createAccountButton.interactable = IsCreateAccountButtonActive();
        }

        public override void Show()
        {
            base.Show();
            SetWaitState(false);
            HideError();
        }

        public void ShowError(string errorMessage)
        {
            _errorText.text = errorMessage;
            _errorBlock.gameObject.SetActive(!string.IsNullOrEmpty(errorMessage));
        }

        public void HideError()
        {
            ShowError(string.Empty);
        }

        public void SetWaitState(bool isWait)
        {
            _createAccountButton.interactable = !isWait;
            _errorBlock.gameObject.SetActive(!isWait);

            LoadingSpinner.SetActiveSpinner(isWait);
        }

        public void ClearFields()
        {
            _accountName.text = string.Empty;
            _passwordField.text = string.Empty;
            _errorText.text = string.Empty;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _emailField.onEndEdit.AddListener(OnEmailEnter);
            _accountName.onEndEdit.AddListener(OnAccountNameEnter);
            _passwordField.onEndEdit.AddListener(OnPasswordEnter);
            _reEnterPassword.onEndEdit.AddListener(OnReEnterPasswordEnter);
            _createAccountButton.onClick.AddListener(OnCreateAccountClicked);
            _stepOneButton.onClick.AddListener(OnBackClicked);
            _backButton.onClick.AddListener(OnBackClicked);
            _closeButton.onClick.AddListener(OnCloseClicked);

            _termsOfUseToggle.onValueChanged.AddListener(OnTermsOfUseToggleCheck);
            _termOfUseButton.onClick.AddListener(OnTermOfUseClicked);
            _privacyPolicyButton.onClick.AddListener(OnPrivacyPolicyClicked);
            _signInButton.onClick.AddListener(OnSignInClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _emailField.onEndEdit.RemoveListener(OnEmailEnter);
            _accountName.onEndEdit.RemoveListener(OnAccountNameEnter);
            _passwordField.onEndEdit.RemoveListener(OnPasswordEnter);
            _reEnterPassword.onEndEdit.RemoveListener(OnReEnterPasswordEnter);
            _createAccountButton.onClick.RemoveListener(OnCreateAccountClicked);
            _stepOneButton.onClick.RemoveListener(OnBackClicked);
            _backButton.onClick.RemoveListener(OnBackClicked);
            _closeButton.onClick.RemoveListener(OnCloseClicked);

            _termsOfUseToggle.onValueChanged.RemoveListener(OnTermsOfUseToggleCheck);
            _termOfUseButton.onClick.RemoveListener(OnTermOfUseClicked);
            _privacyPolicyButton.onClick.RemoveListener(OnPrivacyPolicyClicked);
            _signInButton.onClick.AddListener(OnSignInClicked);
        }

        private void OnSignInClicked()
        {
            SignInClicked.SafeRaise();
        }

        private void OnEmailEnter(string value)
        {
            OnEmailEntered.SafeRaise(value);
        }

        private void OnTermsOfUseToggleCheck(bool value)
        {
            OnTermsOfUseToggleChecked.SafeRaise(value);
        }

        private void OnTermOfUseClicked()
        {
            TermOfUseClicked.SafeRaise();
        }

        private void OnPrivacyPolicyClicked()
        {
            PrivacyPolicyClicked.SafeRaise();
        }

        private void OnCloseClicked()
        {
            CloseClicked.SafeRaise();
        }

        private void OnBackClicked()
        {
            BackClicked.SafeRaise();
        }

        private void OnCreateAccountClicked()
        {
            CreateAccountClicked.SafeRaise();
        }

        private void OnReEnterPasswordEnter(string value)
        {
            OnReEnterPasswordEntered.SafeRaise(value);
        }

        private void OnPasswordEnter(string value)
        {
            OnPasswordEntered.SafeRaise(value);
        }

        private void OnAccountNameEnter(string value)
        {
            OnAccountNameEntered.SafeRaise(value);
        }

        private bool IsCreateAccountButtonActive()
        {
            MarketRegistrationData registrationData = FormModel.RegistrationData;
            return !string.IsNullOrEmpty(registrationData.Email) 
                   && !string.IsNullOrEmpty(registrationData.Password) 
                   && !string.IsNullOrEmpty(registrationData.ReEnterPassword)
                   && registrationData.TermsOfUseIsEnabled;
        }
    }
}