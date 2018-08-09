using SHLibrary.StateMachine;
using TankGame.GameServer.Commands.Battle;
using TankGame.GameServer.Commands.DMarket;
using TankGame.GameServer.Commands.Inventory;
using TankGame.GameServer.Commands.Local;

namespace TankGame.GameServer.States
{
    public class ServerIdleState : StateBase<ServerSceneController>
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);

            ApplyGameBattleCommands();
            ApplyInventoryCommands();
            ApplyForLocalUsingCommands();
            ApplyDmarketCommands();
        }

        private void ApplyDmarketCommands()
        {
            ApplyCommand(new ServerDMarketIntegrationCommand());
            ApplyCommand(new ServerDMarketMovingAssetsCommand());
        }

        private void ApplyForLocalUsingCommands()
        {
            ApplyCommand(new ServerShowReceivedMessageCommand(Controller.LogMessagesText));
        }

        private void ApplyGameBattleCommands()
        {
            ApplyCommand(new ServerBattleUserRegistrationCommand());
            ApplyCommand(new ServerBattleItemDroppingCommand());
            ApplyCommand(new ServerBattleCommand());
        }

        private void ApplyInventoryCommands()
        {
            ApplyCommand(new ServerAppDataCommand());
            ApplyCommand(new ServerItemsOperationCommand());
        }
    }
}