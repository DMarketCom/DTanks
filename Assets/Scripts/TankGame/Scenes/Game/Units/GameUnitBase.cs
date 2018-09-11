using SHLibrary;

namespace Game
{
    public abstract class GameUnitBase : UnityBehaviourBase
    {
        public int UnitId { get; private set; }

        public void Initialize(int unitId)
        {
            UnitId = unitId;
        }
    }
}
