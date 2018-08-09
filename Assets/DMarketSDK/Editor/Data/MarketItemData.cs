using Newtonsoft.Json;
using UnityEngine;

namespace DMarketSDK.Editor
{
    public class MarketItemData
    {
        [JsonProperty(PropertyName = "id")]
        [Header("Type of your item")]
        public string Id;

        [JsonProperty(PropertyName = "meta")]
        public MarketItemMetaData MetaData;

        public MarketItemData()
        {
            Id = "Item type";
            MetaData = new MarketItemMetaData();
        }
    }
}