using TankGame.Network.Messages;

namespace TankGame.GameServer.Commands.Battle
{
    public class ServerBattleCommand : ServerBattleCommandBase
    {
        protected override void OnGameMsgReceived(GameMessageBase message)
        {
            switch (message.Type)
            {
                case GameMsgType.UnitMoved:
                    UpdateTankPos(message as UnitMovedMsg);
                    break;
                case GameMsgType.TankStateUpdate:
                    UpdateTankState(message as TankStateUpdateMsg);
                    break;
                case GameMsgType.Died:
                    OnTankDied(message as TankDiedMsg);
                    break;
                default:
                    Server.SendToAllExcept(message, message.ClientId);
                    break;
            }
        }

        private void UpdateTankPos(UnitMovedMsg message)
        {
            Model.UnitsInBattle[message.ClientId].Position = message.Pos;
            Server.SendToAllExcept(message, message.ClientId);
        }

        private void UpdateTankState(TankStateUpdateMsg message)
        {
            Model.UnitsInBattle[message.ClientId].Position = message.Pos;
            Server.SendToAllExcept(message, message.ClientId);
        }

        private void OnTankDied(TankDiedMsg message)
        {
            Model.UnitsInBattle[message.ClientId].IsAlive = false;
            Server.SendToAllExcept(message, message.ClientId);
        }
    }
}