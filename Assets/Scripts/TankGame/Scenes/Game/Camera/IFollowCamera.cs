using Game.Tank;

namespace TankGame.GameClient.Camera
{
    public interface IFollowCamera
    {
        void SetTarget(ITank target);
    }
}