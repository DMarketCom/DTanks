using DMarketSDK.Market.Domain;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Market.Items.Components
{
    /// <summary>
    /// TODO: Can be extended to other overlay triggers like price, fee, etc. 
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ItemOverlayByStatusComponent : ItemComponentBase
    {
        [SerializeField] private List<SellOfferStatusType> _triggerStatuses;

        private Image _overlayImage;

        public override bool IsNeedBlockInput
        {
            get
            {
                return IsHaveBlockStatus;
            }
        }

        private bool IsHaveBlockStatus
        {
            get { return _triggerStatuses.Contains(Target.Model.Status); }
}

        public override void ApplyItem(MarketItemView item)
        {
            base.ApplyItem(item);
            _overlayImage = GetComponent<Image>();
        }

        public override void ModelUpdate()
        {
            base.ModelUpdate();

            bool needOverlay = IsHaveBlockStatus;
            _overlayImage.enabled = needOverlay;
        }
    }
}