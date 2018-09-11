using DMarketSDK.WidgetCore.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Basic.Forms
{
    public sealed class WidgetLoggedForm : WidgetFormViewBase<WidgetLoggedFormModel>
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
            UpdateUserNameUI();
        }

        private void UpdateUserNameUI()
        {
            _loginText.text = string.IsNullOrEmpty(FormModel.UserName) ? "Logged" : string.Format("Logged as {0}", FormModel.UserName);
        }

        protected override void OnModelChanged()
        { 
            UpdateUserNameUI();
        }
    }
}