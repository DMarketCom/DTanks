using DMarketSDK.Common.UI;
using UnityEngine;

namespace TankGame.Forms
{
    public sealed class WaitingForm : FormBase
    {
        [SerializeField]
        private LoadingSpinner _loadingSpinner;

        public override void Show()
        {
            base.Show();

            _loadingSpinner.StartRunning();
        }

        public override void Hide()
        {
            base.Hide();

            _loadingSpinner.StopRunning();
        }
    }
}
