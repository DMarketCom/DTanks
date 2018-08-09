using TankGame.GameServer.ServerStorage;
using TankGame.Network.Messages;
using TankGame.Network.Server;

namespace TankGame.GameServer.Commands.Battle
{
    public abstract class ServerBattleCommandBase : ServerCommandBase
    {
        protected IGameServer Server { get { return Controller.GameServer; } }
        protected IServerStorage Storage { get { return Controller.Storage; } }

        public override void Start()
        {
            base.Start();
            Server.GameMsgReceived += OnGameMsgReceived;
        }

        protected override void Finish()
        {
            base.Finish();
            Server.GameMsgReceived -= OnGameMsgReceived;
        }

        protected abstract void OnGameMsgReceived(GameMessageBase message);

    }
}