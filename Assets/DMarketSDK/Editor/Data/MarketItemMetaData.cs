using Newtonsoft.Json;
using System.Collections.Generic;

namespace DMarketSDK.Editor
{
    public class MarketItemMetaData
    {
        [JsonProperty(PropertyName = "title")]
        public string Title;

        [JsonProperty(PropertyName = "description")]
        public string Description;

        [JsonProperty(PropertyName = "tags")]
        public List<string> Tags;

        [JsonProperty(PropertyName = "images")]
        public List<MarketItemImageData> ItemImages = new List<MarketItemImageData>();

        public MarketItemMetaData()
        {
            Title = string.Empty;
            Description = string.Empty;
            Tags = new List<string>();
            Tags.Add("default_tag");
            ItemImages = new List<MarketItemImageData>();
            var imageData = new MarketItemImageData();
            imageData.Type = "item";
            ItemImages.Add(imageData);
        }
    }
}