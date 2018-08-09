using DMarketSDK.Basic;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.IntegrationAPI.Settings;
using DMarketSDK.Market;
using SHLibrary.StateMachine;
using TankGame.Application.States;
using TankGame.Catalogs.Scene;
using TankGame.DMarketIntegration;
using TankGame.GameServer.ServerStorage;
using TankGame.Network.Client;
using TankGame.Network.Server;
using UnityEngine;

namespace TankGame.Application
{
    public class AppController : StateMachineBase
    {
        public AppModel Model { get; set; }

        public CommonServer Server { get; set; }

        public CommonClient Client { get; set; }

        [HideInInspector]
        public IServerStorage Storage;
        [SerializeField]
        public ScenesCatalog ScenesCatalog;
        [SerializeField]
        public ServerStorageContainer StorageContainer;

        [SerializeField]
        public ClientApi WidgetApi;
        [SerializeField]
        public MarketApiSettings MarketSettings;

        [SerializeField]
        private BasicWidgetController _basicWidget;

        [SerializeField]
        private MarketWidgetController _marketWidget;

        public bool IsBasicIntegration { get { return _widgetType != IntegrationType.Market; } }

        public IWidgetCore Widget
        {
            get
            {
                if (IsBasicIntegration)
                {
                    return _basicWidget;
                }

                return _marketWidget;
            }
        }

        [SerializeField]
        private IntegrationType _widgetType;

        protected override void Start()
        {
            base.Start();
            DontDestroyOnLoad(gameObject);
            Storage = StorageContainer.Storage;
            SetMarketWidgetType();
            ApplyState<AppInitState>();
        }

        private void SetMarketWidgetType()
        {
            if (_widgetType == IntegrationType.Auto)
            {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
               _widgetType = IntegrationType.Basic;
#else
               _widgetType = IntegrationType.Market;
#endif
            }
        }
    }
}