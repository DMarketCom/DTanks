using Game.Tank;
using System.Collections.Generic;
using TankGame.Domain.GameItem;
using UnityEngine;

namespace Game.States
{
    public abstract class GameInitStateBase : GameStateBase
    {
        protected void RespawnPlayer(Vector3 respawnPoint, List<GameItemType> currentItem)
        {
            if (Controller.Player == null)
            {
                Controller.Player = Context.CreateTank(true, Model.Mode, currentItem);
            }

            Controller.Player.Respawn(respawnPoint);

            Context.FollowCamera.SetTarget(Controller.Player);
        }

        protected void CreateBots(int number)
        {
            Controller.Opponents = new List<ITank>();
            for (int i = 0; i < number; i++)
            {
                var oponent = Context.CreateTank(false, Model.Mode, new List<GameItemType> { GameItemTypeExtensions.GetRandomItem(GameItemCategory.Skin) });
                oponent.Respawn(Context.BattleField.GetSpawnPoint());
                Controller.Opponents.Add(oponent);
            }
        }
    }
}