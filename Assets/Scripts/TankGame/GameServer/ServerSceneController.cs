using UnityEngine.UI;
using Networking.Server;
using PlayerData;
using SHLibrary.StateMachine;
using GameServer.States;
using UnityEngine;
using DMarketSDK.IntegrationAPI;
using Game.BattleField;
using DMarketSDK.Common.ErrorHelper;

namespace GameServer
{
    public class ServerSceneController : StateMachineBase
    {
        [SerializeField]
        public Text TxtMessages;
        [SerializeField]
        public Button BtnBack;
        [SerializeField]
        public ServerApi DMarkteServerApi;

        [SerializeField]
        private BattleFieldController _battleField;
        [SerializeField]
        private ApiErrorHelper _apiErrorCatalog;

        public IAppServer AppServer { get; private set; }
        public IGameServer GameServer { get; private set; }
        public IServerStorage Storage { get; private set; }
        public ServerModel Model { get; private set; }
        public IBattleField BattleField { get { return _battleField; } }
        public IApiErrorHelper ApiErrorHelper { get { return _apiErrorCatalog; } }

        public void Run(IAppServer appServer, IGameServer gameServer, 
            IServerStorage storage)
        {
            AppServer = appServer;
            GameServer = gameServer;
            Storage = storage;
            Model = new ServerModel();
            ApplyState<ServerIdleState>();
        }

        public void Shutdown()
        {
            ApplyState<ServerCloseState>();
        }
    }
}