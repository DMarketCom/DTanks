using Game.Units.Components;
using System;
using UnityEngine;

namespace Game.Tank
{
    public interface ITank
    {
        event Action<ITank> Died;
        event Action<ITank> Moved;

        Vector3 Pos { get; set; }

        float Rotation { get; set; }

        void Respawn(Vector3 point);

        bool IsAlive { get; }

        void Broke();

        GameUnitBase Unit { set; get; }
        IHealtInsideComponent Healt { set; get; }
        IUnitInsideInputComponent Input { set; get; }
        IWeaponInsideComponent Weapon { set; get; }  
    }
}