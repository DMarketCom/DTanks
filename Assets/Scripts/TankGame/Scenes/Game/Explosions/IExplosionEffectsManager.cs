using UnityEngine;

namespace Game.Explosions
{
    public interface IExplosionEffectsManager
    {
        void Play(Vector3 pos, ExplosionEffectType effectType);
    }
}