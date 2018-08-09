using Newtonsoft.Json;
using System.Collections.Generic;

namespace DMarketSDK.IntegrationAPI.Request
{
    public class AsyncMarketResponse
    {
        public class ItemsAsset
        {
            [JsonProperty(PropertyName = "assetId")]
            public string AssetId;
            [JsonProperty(PropertyName = "operationId")]
            public string OperationId;
        }
        public List<ItemsAsset> Items;

        public int totalProcessed;
        public int totalReceived;
    }
}
