using Game.Tank;

namespace Game.Camera
{
    public interface IFollowCamera
    {
        void SetTarget(ITank target);
    }
}