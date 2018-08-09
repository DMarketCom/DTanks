using Game.Units.Components;
using SHLibrary;

namespace Game
{
    public abstract class GameUnitBase : UnityBehaviourBase, IHealtOutsideComponent
    {
        public abstract void TakeDamage(float damage, int fromUnitID);

        public int UnitID { get; private set; }

        public void Initialize(int unitId)
        {
            UnitID = unitId;
        }
    }
}
