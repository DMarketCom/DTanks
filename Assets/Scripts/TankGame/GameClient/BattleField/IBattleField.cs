using UnityEngine;

namespace Game.BattleField
{
    public interface IBattleField
    {
        int TotalPointsForTankSpawn { get; }
        int TotalPointsForItemSpawn { get; }

        Vector3 GetSpawnPoint();
        Vector3 GetSpawnPoint(int spawnPointIndex);
        Vector3 GetPointForItem(int pointIndex);
    }
}
