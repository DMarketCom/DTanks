using SHLibrary.Logging;
using System;
using TMPro;
using UnityEngine;
using DMarketSDK.Market.Domain;

namespace DMarketSDK.Market.Items.Components
{
    [RequireComponent(typeof(TextMeshProUGUI)),]
    public abstract class ItemShowTextComponentBase : ItemComponentBase
    {
        protected enum ShowType
        {
            Tittle = 0,
            AssetId = 1,
            ClassId = 2,
            CreationDate = 3,
            Price = 4,
            Fee = 5,
            OrderStatus = 6,
			Description = 7,
			OffersCount = 8,
            LastUpdate = 9,
            SellOfferId = 10
        }

        private TextMeshProUGUI _text;
        
        protected string ShowFormat { get; private set; }

        public override void ApplyItem(MarketItemView item)
        {
            base.ApplyItem(item);
            if (_text == null)
            {
                _text = GetComponent<TextMeshProUGUI>();
                ShowFormat = _text.text;
            }
        }

        public override void ModelUpdate()
        {
            base.ModelUpdate();
            _text.text = GetShowingText();
        }

        protected abstract string GetShowingText();

        protected string GetTargetString(ShowType showType, MarketItemModel model)
        {
            var result = string.Empty;
            switch (showType)
            {
                case ShowType.Tittle:
                    result = model.Tittle;
                    break;
				case ShowType.Description:
					result = model.Description;
					break;
                case ShowType.ClassId:
                    result = model.ClassId;
                    break;
                case ShowType.AssetId:
                    result = model.AssetId;
                    break;
                case ShowType.CreationDate:
                    result = GetDateString(model.Created);
                    break;
                case ShowType.LastUpdate:
                    result = GetDateString(model.Updated);
                    break;
                case ShowType.Price:
                    result = model.Price.GetStringWithCurrencySprite(_text.color);
                    break;
                case ShowType.Fee:
                    result = model.Fee.GetStringWithCurrencySprite(_text.color);
                    break;
                case ShowType.OrderStatus:
                    result = model.Status.GetText();
                    break;
				case ShowType.OffersCount:
					result = model.OffersCount.ToString();
					break;
                case ShowType.SellOfferId:
                    result = model.SellOfferId;
                    break;
                default:
                    result = "NOT IMPLEMENT";
                    DevLogger.Warning("Need implement for " + showType);
                    break;
            }
            return result;
        }

        private static string GetDateString(DateTime date)
        {
            return date.ToString("MMMM dd H:mm:ss");
        }
    }
}