using DMarketSDK.Common.UI;
using UnityEngine;

namespace TankGame.UI.Forms
{
    public sealed class WaitingForm : FormBase
    {
        [SerializeField]
        private LoadingSpinner _loadingSpinner;

        public override void Show()
        {
            base.Show();

            _loadingSpinner.SetActiveSpinner(true);
        }

        public override void Hide()
        {
            base.Hide();

            _loadingSpinner.SetActiveSpinner(false);
        }
    }
}
