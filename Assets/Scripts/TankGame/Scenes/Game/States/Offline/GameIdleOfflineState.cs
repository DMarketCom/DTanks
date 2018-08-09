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
            var diedOponent = Controller.Opponents.Find(oponent => !oponent.IsAlive);
            if (diedOponent != null)
            {
                diedOponent.Respawn(Context.BattleField.GetSpawnPoint());
            }
        }

        protected override void OnPlayerDied(ITank tank)
        {
            base.OnPlayerDied(tank);
            ApplyState<GameEndOfflineState>();
        }
    }
}