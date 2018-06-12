using Networking.Msg;

namespace GameServer.Commands
{
    public class ServerBattleCommand : ServerBattleCommandBase
    {
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
        
        protected override void OnGameMsgReceived(GameMessageBase message)
        {
            switch (message.Type)
            {
                case GameMsgType.UnitMoved:
                    UpdateTankPos(message as UnitMovedMsg);
                    break;
                case GameMsgType.TankStateUpdate:
                    UpdatTankState(message as TankStateUpdateMsg);
                    break;
                case GameMsgType.Died:
                    OnTankDied(message as TankDiedMsg);
                    break;
                default:
                    Server.SendToAllExept(message, message.ClientId);
                    break;
            }
        }

        private void UpdateTankPos(UnitMovedMsg message)
        {
            Model.UnitsInBattle[message.ClientId].Position = message.Pos;
            Server.SendToAllExept(message, message.ClientId);
        }

        private void UpdatTankState(TankStateUpdateMsg message)
        {
            Model.UnitsInBattle[message.ClientId].Position = message.Pos;
            Server.SendToAllExept(message, message.ClientId);
        }

        private void OnTankDied(TankDiedMsg message)
        {
            Model.UnitsInBattle[message.ClientId].IsAlive = false;
            Server.SendToAllExept(message, message.ClientId);
        }
    }
}