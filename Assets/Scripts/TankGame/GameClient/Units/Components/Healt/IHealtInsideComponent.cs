using System;

namespace Game.Units.Components
{
    public interface IHealtInsideComponent
    {
        event Action<float> Damaged;
    }
}