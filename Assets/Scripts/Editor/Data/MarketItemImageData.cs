using Newtonsoft.Json;

namespace DMarketSDK.Editor
{
    public class MarketItemImageData
    {
        [JsonProperty(PropertyName = "image", Order = 0)]
        public string ImageUrl;

        [JsonProperty(PropertyName = "type", Order = 1)]
        public string Type;
    }
}