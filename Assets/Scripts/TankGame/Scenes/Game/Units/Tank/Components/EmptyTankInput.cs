using System;
using UnityEngine;
using SHLibrary;

namespace Game.Units.Components
{
    public class EmptyTankInput : UnityBehaviourBase, IUnitInsideInputComponent
    {
        public event Action<Vector2> Move;
        public event Action<Vector2, float> Fire;

        public void MakeMove(Vector2 moveDir)
        {
            Move.SafeRaise(moveDir);
        }

        public void MakeFire(Vector2 dir, float force)
        {
            Fire.SafeRaise(dir, force);
        }
    }
}