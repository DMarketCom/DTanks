using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Widget.Forms
{
    public class WidgetAutorizationFormBase : WidgetFormBase
    {
        public InputField LoginField;
        public InputField PasswordField;
        public Text ErrorText;
        public Button SendButton;
        public GameObject LoadingAnimation;

        public override void Show()
        {
            base.Show();
            ClearFields();
            SetWaitState(false);
            HideError();
        }

        public void ShowError(string errorMessage)
        {
            ErrorText.text = errorMessage;
            ErrorText.gameObject.SetActive(!string.IsNullOrEmpty(errorMessage));
        }

        public void HideError()
        {
            ShowError(string.Empty);
        }

        public void SetWaitState(bool wait = true)
        {
            SendButton.interactable = !wait;
            ErrorText.gameObject.SetActive(!wait);
            LoadingAnimation.SetActive(wait);
        }

        public virtual void ClearFields()
        {
            LoginField.text = string.Empty;
            PasswordField.text = string.Empty;
            ErrorText.text = string.Empty;
        }
    }
}