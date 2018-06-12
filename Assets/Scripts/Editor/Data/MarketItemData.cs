using Newtonsoft.Json;

namespace DMarketSDK.Editor
{
    public class MarketItemData
    {
        [JsonProperty(PropertyName = "id", Order = 0)]
        public string Id;

        [JsonProperty(PropertyName = "meta", Order = 1)]
        public MarketItemMetaData MetaData = new MarketItemMetaData();
    }
}