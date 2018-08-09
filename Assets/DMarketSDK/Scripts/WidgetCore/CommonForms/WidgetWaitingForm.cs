using DMarketSDK.Common.UI;
using UnityEngine;

namespace DMarketSDK.WidgetCore.Forms
{
    public class WidgetWaitingForm : WidgetFormViewBase
    {
        [SerializeField]
        private LoadingSpinner _loadingSpinner;

        private object _key;

        protected override void OnModelChanged()
        {
            
        }

        protected override void OnEnable()
        {
            _loadingSpinner.SetActiveSpinner(true);
        }

        protected override void OnDisable()
        {
            _loadingSpinner.SetActiveSpinner(false);
        }

        public void Show(object key)
        {
            _key = key;
            Show();
        }

        public void Hide(object key)
        {
            if (_key.Equals(key))
            {
                Hide();
            }
        }
    }
}