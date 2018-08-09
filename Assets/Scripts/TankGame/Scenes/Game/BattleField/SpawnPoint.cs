using SHLibrary;
using UnityEngine;

namespace TankGame.BattleField
{
    public class SpawnPoint : UnityBehaviourBase
    {
        public Vector3 Pos { get { return transform.position; } }
    }
}