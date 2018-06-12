
using SHLibrary;
using UnityEngine;

namespace Game.BattleField
{
    public class SpawnPoint : UnityBehaviourBase
    {
        public Vector3 Pos { get { return transform.position; } }

        public bool IsFree()
        {
            return true;
        }
    }
}