using SHLibrary.ObserverView;

namespace Game.Tank
{
    public class TankModel : ObservableBase
    {
        public readonly float MaxHealth = 3f;

        public float Health;

        public float MoveSpeed { get { return 5f; } }

        public float RotateSpeed { get { return 90f; } }

        public TankModel()
        {
            Health = MaxHealth;
        }
    }
}
