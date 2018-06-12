using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.Forms
{
    public class AuthorizationFormBase : FormBase
    {
        [SerializeField]
        private  MessageBoxForm _messageBoxForm;

        [SerializeField]
        private WaitingForm _waitingForm;

        public TMP_InputField LoginField;
        public TMP_InputField PasswordField;
        public Button SendButton;

        public void ShowError(string errorMessage)
        {
            _messageBoxForm.Show("Error", errorMessage);
            _messageBoxForm.Show();
        }

        public void ShowWaitingForm()
        {
            _waitingForm.Show();
            SendButton.interactable = false;
        }

        public void HideWaitingForm()
        {
            _waitingForm.Hide();
            SendButton.interactable = true;
        }
    }
}