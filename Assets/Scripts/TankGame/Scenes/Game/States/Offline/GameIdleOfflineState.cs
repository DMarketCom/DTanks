using Game.Tank;

namespace Game.States.Offline
{
    public class GameIdleOfflineState : GameIdleStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            ScheduledUpdate(10f, true);
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();
            var diedOpponent = Controller.Opponents.Find(opponent => !opponent.IsAlive);
            if (diedOpponent != null)
            {
                diedOpponent.Respawn(Context.BattleField.GetSpawnPoint());
            }
        }

        protected override void OnPlayerDied(ITank tank)
        {
            base.OnPlayerDied(tank);
            ApplyState<GameEndOfflineState>();
        }
    }
}