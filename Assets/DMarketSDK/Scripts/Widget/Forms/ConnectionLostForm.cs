using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Widget.Forms
{
    public class ConnectionLostForm : WidgetFormBase
    {
        [SerializeField]
        public Button BtnRetry;
        [SerializeField]
        public Text TxtMsg;
    }
}