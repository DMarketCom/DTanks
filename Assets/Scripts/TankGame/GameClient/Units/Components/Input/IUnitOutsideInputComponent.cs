using System;
using UnityEngine;

namespace Game.Units.Components
{
    public interface IUnitInsideInputComponent
    {
        event Action<Vector2> Move;
        event Action<Vector2, float> Fire;
    }
}