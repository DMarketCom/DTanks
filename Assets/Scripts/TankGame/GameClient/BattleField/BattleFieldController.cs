
using SHLibrary;
using SHLibrary.Logging;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BattleField
{
    public class BattleFieldController : UnityBehaviourBase, IBattleField
    {
        [SerializeField]
        private List<SpawnPoint> _spawnPoints;
        [SerializeField]
        private List<SpawnPoint> _itemsPoints;

        #region IBattleField implementation
        int IBattleField.TotalPointsForTankSpawn
        {
            get
            {
                return _spawnPoints.Count;
            }
        }

        int IBattleField.TotalPointsForItemSpawn
        {
            get
            {
                return _itemsPoints.Count;
            }
        }

        Vector3 IBattleField.GetPointForItem(int pointIndex)
        {
            return _itemsPoints[pointIndex].Pos;
        }

        Vector3 IBattleField.GetSpawnPoint()
        {
            var random = Random.Range(0, _spawnPoints.Count);
            return _spawnPoints[random].Pos;
        }

        Vector3 IBattleField.GetSpawnPoint(int spawnPointIndex)
        {
            if (spawnPointIndex >= _spawnPoints.Count)
            {
                DevLogger.Warning("spawn point index {0} not exist", TankGameLogChannel.GameClient);
                spawnPointIndex = 0;
            }
            return _spawnPoints[spawnPointIndex].Pos;
        }
        #endregion
    }
}