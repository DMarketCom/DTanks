using System;
using DMarketSDK.WidgetCore.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Forms
{
    public class RegisterStepOneForm : WidgetFormViewBase<RegistrationFormModel>
    {
        public event Action<string> OnEmailEntered;
        public event Action<bool> OnTermsOfUseToggleChecked;
        public event Action<bool> OnNewsletterToggleChecked;
        public event Action NextClicked;
        public event Action BackClicked;
        public event Action TermOfUseClicked;
        public event Action PrivacyPolicyClicked;

        [SerializeField]
        private TMP_InputField _emailField;

        [SerializeField]
        private Toggle _termsOfUseToggle;

        [SerializeField]
        private Toggle _newsletterToggle;

        [SerializeField]
        private GameObject _errorBlock;

        [SerializeField]
        private TextMeshProUGUI _errorText;

        [SerializeField]
        private Button _nextBtn;

        [SerializeField]
        private Button _stepTwoButton;

        [SerializeField]
        private Button _termOfUseButton;

        [SerializeField]
        private Button _privacyPolicyButton;

        [SerializeField]
        private Button _backButton;

        public void ShowError(string errorMessage)
        {
            _errorText.text = errorMessage;
            _errorBlock.gameObject.SetActive(!string.IsNullOrEmpty(errorMessage));
        }

        public void HideError()
        {
            ShowError(string.Empty);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _emailField.onEndEdit.AddListener(OnEmailEnter);
            _termsOfUseToggle.onValueChanged.AddListener(OnTermsOfUseToggleCheck);
            _newsletterToggle.onValueChanged.AddListener(OnNewsletterToggleCheck);
            _nextBtn.onClick.AddListener(OnNextClicked);
            _stepTwoButton.onClick.AddListener(OnStepTwoClicked);
            _termOfUseButton.onClick.AddListener(OnTermOfUseClicked);
            _privacyPolicyButton.onClick.AddListener(OnPrivacyPolicyClicked);
            _backButton.onClick.AddListener(OnBackClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _emailField.onEndEdit.RemoveListener(OnEmailEnter);
            _termsOfUseToggle.onValueChanged.RemoveListener(OnTermsOfUseToggleCheck);
            _newsletterToggle.onValueChanged.RemoveListener(OnNewsletterToggleCheck);
            _nextBtn.onClick.RemoveListener(OnNextClicked);
            _stepTwoButton.onClick.RemoveListener(OnStepTwoClicked);
            _termOfUseButton.onClick.RemoveListener(OnTermOfUseClicked);
            _privacyPolicyButton.onClick.RemoveListener(OnPrivacyPolicyClicked);
            _backButton.onClick.RemoveListener(OnBackClicked);
        }

        private void OnNextClicked()
        {
            NextClicked.SafeRaise();
        }

        private void OnStepTwoClicked()
        {
            NextClicked.SafeRaise();
        }

        private void OnEmailEnter(string value)
        {
            OnEmailEntered.SafeRaise(value);
        }

        private void OnTermsOfUseToggleCheck(bool value)
        {
            OnTermsOfUseToggleChecked.SafeRaise(value);
        }

        private void OnNewsletterToggleCheck(bool value)
        {
            OnNewsletterToggleChecked.SafeRaise(value);
        } 

        private void OnTermOfUseClicked()
        {
            TermOfUseClicked.SafeRaise();
        }

        private void OnPrivacyPolicyClicked()
        {
            PrivacyPolicyClicked.SafeRaise();
        }

        private void OnBackClicked()
        {
            BackClicked.SafeRaise();
        }

        protected override void OnModelChanged()
        {
            _emailField.text = FormModel.RegistrationData.Email;
            _termsOfUseToggle.isOn = FormModel.RegistrationData.TermsOfUseIsEnabled;
            _newsletterToggle.isOn = FormModel.RegistrationData.NewsletterIsEnabled;

            if (_termsOfUseToggle.IsActive())
            {
                _nextBtn.interactable = FormModel.RegistrationData.TermsOfUseIsEnabled;
                _stepTwoButton.interactable = FormModel.RegistrationData.TermsOfUseIsEnabled;
            }
            else
            {
                _nextBtn.interactable = true;
                _stepTwoButton.interactable = true;
            }
        }
    }
}