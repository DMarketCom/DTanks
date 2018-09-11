using System;

namespace Game.Units.Components
{
    public interface IHealthComponent
    {
        event Action<float> Damaged;

        void TakeDamage(float damage, int unitID);
    }
}