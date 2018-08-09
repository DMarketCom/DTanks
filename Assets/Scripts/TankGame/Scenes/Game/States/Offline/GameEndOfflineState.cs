using TankGame.GameClient.Commands;

namespace Game.States.Offline
{
    public class GameEndOfflineState : GameStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            ApplyCommand(new ShowGameOverPopUpCommand());
        }
    }
}