using GameServer.Commands;
using SHLibrary.StateMachine;

namespace GameServer.States
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
        }

        private void ApplyForLocalUsingCommands()
        {
            ApplyCommand(new ServerShowReceivedMessageCommand(Controller.TxtMessages));
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