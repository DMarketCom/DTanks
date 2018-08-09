using DMarketSDK.IntegrationAPI.Internal;
using DMarketSDK.IntegrationAPI.Settings;
using UnityEngine;

namespace DMarketSDK.IntegrationAPI
{
    public abstract class BaseApi<T> : MonoBehaviour
        where T : IBaseApiSettings
    {
        protected IApiTransport ApiTransport { get; private set; }

        protected T ApiSettings { get; private set; }

        public void ApplyHttpProtocol(T apiSettings)
        {
            var httpTransport = gameObject.AddComponent<HttpApiTransport>();
            ApplyCustomProtocol(apiSettings, httpTransport);
        }

        public void ApplyCustomProtocol(T apiSettings, IApiTransport apiTransport)
        {
            if (ApiTransport != null)
            {
                ApiTransport.Dispose();
            }
            ApiSettings = apiSettings;
            ApiTransport = apiTransport;
            ApiTransport.Initialize(apiSettings);
        }
    }
}