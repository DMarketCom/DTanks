using Game.Units.Components;
using System;
using UnityEngine;

namespace Game.Tank
{
    public interface ITank
    {
        event Action<ITank> Died;
        event Action<ITank> Moved;

        Vector3 Position { get; set; }

        float Rotation { get; set; }

        void Respawn(Vector3 point);

        bool IsAlive { get; }

        void Broke();

        GameUnitBase Unit { set; get; }
        IHealthComponent Health { set; get; }
        IUnitInsideInputComponent Input { set; get; }
        IWeaponComponent Weapon { set; get; }  
    }
}