using UnityEngine.UI;

namespace DMarketSDK.Widget.Forms
{
    public class WidgetRegisterForm : WidgetAutorizationFormBase
    {
        public InputField EmailField;
        public Button BtnLogin;

        public override void ClearFields()
        {
            base.ClearFields();
            EmailField.text = string.Empty;
        }
    }
}