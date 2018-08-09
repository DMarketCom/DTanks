using DMarketSDK.IntegrationAPI;
using DMarketSDK.IntegrationAPI.Settings;
using SHLibrary.StateMachine;
using TankGame.BattleField;
using TankGame.GameServer.ServerStorage;
using TankGame.GameServer.States;
using TankGame.Network.Server;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.GameServer
{
    public class ServerSceneController : StateMachineBase
    {
        [SerializeField]
        public TextMeshProUGUI LogMessagesText;
        [SerializeField]
        public Button BtnBack;
        [SerializeField]
        public ServerApi DMarketServerApi;

        [SerializeField]
        private MarketApiSettings _apiSettings;
        [SerializeField]
        private BattleFieldController _battleField;

        public IAppServer AppServer { get; private set; }
        public IGameServer GameServer { get; private set; }
        public IServerStorage Storage { get; private set; }
        public ServerModel Model { get; private set; }
        public IBattleField BattleField { get { return _battleField; } }

        public void Run(IAppServer appServer, IGameServer gameServer, 
            IServerStorage storage)
        {
            AppServer = appServer;
            GameServer = gameServer;
            Storage = storage;
            Model = new ServerModel();
            DMarketServerApi.ApplyHttpProtocol(_apiSettings);
            ApplyState<ServerIdleState>();
        }

        public void Shutdown()
        {
            ApplyState<ServerCloseState>();
        }
    }
}