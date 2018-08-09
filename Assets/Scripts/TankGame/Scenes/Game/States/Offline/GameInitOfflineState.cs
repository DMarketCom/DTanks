using System.Collections.Generic;
using TankGame.Domain.GameItem;

namespace Game.States.Offline
{
    public class GameInitOfflineState : GameInitStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            RespawnPlayer(Context.BattleField.GetSpawnPoint(), new List<GameItemType> { GameItemType.SkinYellow });
            CreateBots(8);
            ApplyState<GameIdleOfflineState>();
        }
    }
}