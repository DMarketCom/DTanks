using DMarketSDK.Common.UI;
using DMarketSDK.WidgetCore.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Forms
{
    public sealed class PasswordRecoveryForm : WidgetFormViewBase<RecoveryPasswordFormModel>
    {
        public TMP_InputField EmailField;

        public GameObject ErrorBlock;
        public TextMeshProUGUI ErrorText;

        public Button SendButton;
        public Button CloseButton;
        public Button BackButton;   

        public LoadingSpinner LoadingSpinner;

        public void ShowError(string errorMessage)
        {
            ErrorText.text = errorMessage;
            ErrorBlock.gameObject.SetActive(!string.IsNullOrEmpty(errorMessage));
        }

        public void HideError()
        {
            ShowError(string.Empty);
        }

        public void SetWaitState(bool wait)
        {
            SendButton.interactable = !wait;
            ErrorBlock.gameObject.SetActive(!wait);
            LoadingSpinner.SetActiveSpinner(wait);
        }

        public void ClearFields()
        {
            EmailField.text = string.Empty;
        }

        protected override void OnModelChanged()
        {
            EmailField.text = FormModel.AccountEmail;
        }
    }
}
