using Newtonsoft.Json;

namespace DMarketSDK.Editor
{
    public class MarketItemImageData
    {
        [JsonProperty(PropertyName = "image")]
        public string ImageUrl;

        [JsonProperty(PropertyName = "type")]
        public string Type;

        public MarketItemImageData()
        {
            ImageUrl = string.Empty;
            Type = string.Empty;
        }
    }
}