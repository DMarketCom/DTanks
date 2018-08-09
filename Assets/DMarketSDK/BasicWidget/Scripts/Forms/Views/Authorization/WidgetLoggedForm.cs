using DMarketSDK.Domain;
using DMarketSDK.Forms;
using DMarketSDK.WidgetCore.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace DMarketSDK.Basic.Forms
{
    public sealed class WidgetLoggedForm : WidgetFormViewBase<WidgetLoginFormModel>
    {
        [SerializeField]
        private TextMeshProUGUI _loginText;
        [SerializeField]
        public Button BtnLogout;
        [SerializeField]
        public Button BtnClose;

        public override void Show()
        {
            base.Show();
            ShowUserLogin();
        }

        private void ShowUserLogin()
        {
            if (FormModel == null || string.IsNullOrEmpty(FormModel.UserLogin))
            {
                _loginText.text = "Logged";
            }
            else
            {
                _loginText.text = string.Format("Logged as {0}", FormModel.UserLogin);
            }
        }

        protected override void OnModelChanged()
        { 
            ShowUserLogin();
        }
    }
}