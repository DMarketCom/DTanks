﻿using System;

namespace Game.Units.Components
{
    public class TankHealthComponent : GameUnitBase, IHealtInsideComponent
    {
        #region IHealtInsideComponent implementation
        public event Action<float> Damaged;
        #endregion
        
        public override void TakeDamage(float damage, int unitID)
        {
            Damaged.SafeRaise(damage);
        }
    }
}