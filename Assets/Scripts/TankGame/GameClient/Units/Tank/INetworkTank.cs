using UnityEngine;

namespace Game.Tank
{
    public interface INetworkTank
    {
        void Move(Vector3 pos, float time);
        void Broke();
    }
}
