using DMarketSDK.IntegrationAPI.Request;
using System;
using SHLibrary.ObserverView;
using UnityEngine;
using DMarketSDK.Market.Domain;

namespace DMarketSDK.Market.Items
{
    public class MarketItemModel : ObservableBase
    {
        public string Tittle = string.Empty;
		public string Description = string.Empty;
        public string ImageUrl = string.Empty;
        public string ClassId = string.Empty;
        public string AssetId = string.Empty;
        public string SellOfferId = string.Empty;
        public Sprite IconSprite = null;

        public DateTime Created = new DateTime();
        public DateTime Updated = new DateTime();
        public Price Fee = new Price();
        public Price Price = new Price();
        public SellOfferStatusType Status = SellOfferStatusType.None;
		public long OffersCount = 0;

        public bool IsLockFromServer = false;
        public bool IsSelected = false;

        public override bool Equals(object obj)
        {
            var item = obj as MarketItemModel;
            if (item == null)
            {
                return false;
            }

            return Tittle == item.Tittle
                   && AssetId == item.AssetId
                   && ClassId == item.ClassId
                   && Status == item.Status
                   && Price.Equals(item.Price)
                   && Fee.Equals(item.Fee)
                   && Description == item.Description
                   && Created == item.Created
                   && Updated == item.Updated
                   && OffersCount == item.OffersCount
                   && ImageUrl == item.ImageUrl;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
} 