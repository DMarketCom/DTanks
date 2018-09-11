using TankGame.Network.Messages;

namespace TankGame.GameServer.Commands.Battle
{
    public class ServerBattleCommand : ServerBattleCommandBase
    {
        protected override void OnGameMsgReceived(GameMessageBase message)
        {
            switch (message.Type)
            {
                case GameMsgType.UnitPosition:
                    OnUnitPositionUpdate(message as UnitPositionMessage);
                    break;
                case GameMsgType.UnitDestroy:
                    OnUnitDestroy(message as UnitDestroyMessage);
                    break;
            }
        }

        private void OnUnitPositionUpdate(UnitPositionMessage message)
        {
            GameBattlePlayerInfo battlePlayer = Model.GetBattlePlayer(message.ClientId);
            battlePlayer.Position = message.Position;

            Server.SendToAllExcept(message, message.ClientId);
        }

        private void OnUnitDestroy(UnitDestroyMessage message)
        {
            GameBattlePlayerInfo battlePlayer = Model.GetBattlePlayer(message.ClientId);
            battlePlayer.IsAlive = false;

            Server.SendToAllExcept(message, message.ClientId);
        }
    }
}