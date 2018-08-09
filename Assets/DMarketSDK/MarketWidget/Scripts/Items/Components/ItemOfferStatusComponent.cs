using DMarketSDK.Market.Domain;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DMarketSDK.Market.Items.Components
{
    public sealed class ItemOfferStatusComponent : ItemComponentBase
    {
        [Serializable]
        private struct OfferStatusItem
        {
            public SellOfferStatusType StatusType;
            public Color StatusColor;

            public bool IsNull { get { return StatusType == SellOfferStatusType.None; } }
        }

        [SerializeField]
        private OfferStatusItem _defaultStatusSettings;
        [SerializeField]
        private List<OfferStatusItem> _statusSettings;
        [SerializeField]
        private Image _imgStatus;
        [SerializeField]
        private TextMeshProUGUI _txtStatus;

        public override void ModelUpdate()
        {
            base.ModelUpdate();

            UpdateItemStatus();
        }

        private void UpdateItemStatus()
        {
            var itemStatus = Target.Model.Status;
            if (itemStatus == SellOfferStatusType.None)
            {
                HideStatus();
            }
            else
            {
                var showSettings = GetStatusSetting(itemStatus);
                ShowStatus(showSettings);
            }
        }

        private void ShowStatus(OfferStatusItem showSettings)
        {
            _imgStatus.enabled = true;
            _imgStatus.color = showSettings.StatusColor;
            _txtStatus.color = showSettings.StatusColor;
            _txtStatus.text = showSettings.StatusType.GetText();
        }

        private void HideStatus()
        {
            _imgStatus.enabled = false;
            _txtStatus.text = string.Empty;
        }

        private OfferStatusItem GetStatusSetting(SellOfferStatusType itemStatus)
        {
            var result = _statusSettings.Find(item => item.StatusType == itemStatus);
            if (result.IsNull)
            {
                result = _defaultStatusSettings;
                result.StatusType = itemStatus;
            }
            return result;
        }
    }
}