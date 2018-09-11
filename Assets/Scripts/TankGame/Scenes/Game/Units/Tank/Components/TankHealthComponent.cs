using System;

namespace Game.Units.Components
{
    public class TankHealthComponent : GameUnitBase, IHealthComponent
    {
        #region IHealtInsideComponent implementation

        public event Action<float> Damaged;

        #endregion

        public void TakeDamage(float damage, int unitId)
        {
            Damaged.SafeRaise(damage);
        }
    }
}