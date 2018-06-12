using System;
using DMarketSDK.IntegrationAPI.Internal;
using UnityEngine;

namespace DMarketSDK.IntegrationAPI
{
    public abstract class BaseApi : MonoBehaviour
    {
        //TODO move to catalog settings
        private readonly string _gameToken = "DTanks_API_token";
        private readonly bool _useSandboxEnvironment = true;
        private readonly string _sandboxEnvironmentUrl = "https://gi-sandbox.dmarket.com/";
        private readonly string _productionEnvironmentUrl = string.Empty;

        protected IApiTransport ApiTransport { get; private set; }

        public string GameToken { get { return _gameToken; } }

        private Uri BaseHost()
        {
            return new Uri(_useSandboxEnvironment ? _sandboxEnvironmentUrl : _productionEnvironmentUrl);
        }
        
        private void Start()
        {
            ApiTransport = gameObject.AddComponent<HttpApiTransport>();
            ((HttpApiTransport)ApiTransport).SetBaseHost(BaseHost());
            ((HttpApiTransport)ApiTransport).ToggleDebug(_useSandboxEnvironment);
        }
    }
}