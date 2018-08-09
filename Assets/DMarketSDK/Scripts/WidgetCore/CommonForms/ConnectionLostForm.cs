using DMarketSDK.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.WidgetCore.Forms
{
    public sealed class ConnectionLostForm : WidgetFormViewBase<WidgetFormModel>
    {
        [SerializeField]
        public Button BtnRetry;
        [SerializeField]
        public Text TxtMsg;
    }
}