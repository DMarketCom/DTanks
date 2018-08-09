using DMarketSDK.Common.UI;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Market.Items.Components
{
    [RequireComponent(typeof(Image))]
    public abstract class ItemImageLoadingComponent : ItemComponentBase
    {
        protected Image TargetImage { get; private set; }

        [SerializeField]
        private LoadingSpinner _spinner;

        public override void ApplyItem(MarketItemView item)
        {
            base.ApplyItem(item);
            TargetImage = GetComponent<Image>();
            SetLoadAnim(false);
        }

        protected void ApplyImageChanging()
        {
            SetLoadAnim(TargetImage.sprite == null);
        }

        private void SetLoadAnim(bool turnOn)
        {
            TargetImage.enabled = !turnOn;

            if (_spinner == null)
            {
                return;
            }

            var isLoading = turnOn && !Target.Model.IsLockFromServer;
            _spinner.SetActiveSpinner(isLoading);
        }
    }
}