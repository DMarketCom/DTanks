using System;
using UnityEngine;
using SHLibrary;

namespace Game.Units.Components
{
    public class EmptyTankInput : UnityBehaviourBase, IUnitInsideInputComponent
    {
        public event Action<Vector2> Move;
        public event Action<Vector2, float> Fire;
    }
}