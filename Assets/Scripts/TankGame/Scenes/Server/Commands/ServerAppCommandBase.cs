using SHLibrary.Logging;
using TankGame.Domain.PlayerData;
using TankGame.GameServer.ServerStorage;
using TankGame.Network.Messages;
using TankGame.Network.Server;

namespace TankGame.GameServer.Commands
{
    public abstract class ServerAppCommandBase : ServerCommandBase
    {
        protected IAppServer Server { get { return Controller.AppServer; } }
        protected IServerStorage Storage { get { return Controller.Storage; } }

        protected abstract void OnAppMsgReceived(AppMessageBase message);

        public override void Start()
        {
            base.Start();
            Server.AppMsgReceived += OnAppMsgReceived;
            Server.Disconnected += OnDisconnected;
        }

        protected override void Finish()
        {
            base.Finish();
            Server.AppMsgReceived -= OnAppMsgReceived;
            Server.Disconnected -= OnDisconnected;
        }

        protected void SendMessageToClient(AppServerAnswerMessageBase answerMessage, int connectionId)
        {
            answerMessage.ConnectionId = connectionId;
            var logString = "Server send to {0} app answer type {1} with error {2}";
            DevLogger.Log(string.Format(logString, answerMessage.ConnectionId,
                answerMessage.Type, answerMessage.Error), DTanksLogChannel.Network);
            Server.Send(answerMessage);
        }

        protected PlayerInfo GetPlayer(int connectionId)
        {
            return Storage.Get(Model.GetUserNameByConnectionId(connectionId));
        }

        protected PlayerInventoryInfo GetInventory(int connId)
        {
            return GetPlayer(connId).Inventory;
        }

        protected virtual void OnDisconnected(int connectionId) { }
    }
}
