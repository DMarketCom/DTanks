using Networking.Msg;
using Networking.Server;
using PlayerData;
using SHLibrary.Logging;

namespace GameServer.Commands
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
            Server.Disconected += OnDisconected;
        }

        protected override void Finish()
        {
            base.Finish();
            Server.AppMsgReceived -= OnAppMsgReceived;
            Server.Disconected -= OnDisconected;
        }

        protected void SendAnswer(AppServerAnswerMessageBase answer,
           AppMessageBase senderMessage)
        {
            answer.ConnectionId = senderMessage.ConnectionId;
            var logString = "Server send to {0} app answer type {1} with error {2}";
            DevLogger.Log(string.Format(logString, answer.ConnectionId,
                answer.Type, answer.Error), TankGameLogChannel.Network);
            Server.Send(answer);
        }

        protected PlayerInfo GetPlayer(int connId)
        {
            return Storage.Get(Model.ConIdToUserName[connId]);
        }

        protected PlayerInventoryInfo GetInventory(int connId)
        {
            return GetPlayer(connId).Inventory;
        }

        protected virtual void OnDisconected(int conId)
        {
        }
    }
}
