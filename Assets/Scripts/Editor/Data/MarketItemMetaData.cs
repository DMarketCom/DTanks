using Newtonsoft.Json;
using System.Collections.Generic;

namespace DMarketSDK.Editor
{
    public class MarketItemMetaData
    {
        [JsonProperty(PropertyName = "title", Order = 0)]
        public string Title;

        [JsonProperty(PropertyName = "gameId", Order = 1)]
        public string GameId;

        [JsonProperty(PropertyName = "category", Order = 2)]
        public string Category;

        [JsonProperty(PropertyName = "description", Order = 3)]
        public string Description;

        [JsonProperty(PropertyName = "images", Order = 4)]
        public List<MarketItemImageData> ItemImages = new List<MarketItemImageData>();
    }
}