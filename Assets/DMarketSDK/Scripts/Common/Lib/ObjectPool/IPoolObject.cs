using System;

namespace SHLibrary.ObjectPool
{
    /// <summary>
    ///     Предоставляет любой объект, которым можно взаимодействовать через <see cref="PoolControllerBase" />.
    /// </summary>
    public interface IPoolObject
    {
        /// <summary>
        ///     Вызывается когда объект больше не используется и должен быть возвращен в пул.
        /// </summary>
        event Action<IPoolObject> PoolObjectDestroyed;

        /// <summary>
        ///     Активирует объект.
        /// </summary>
        /// <code>
        /// Например: this.GameObject.SetActive(true);
        /// </code>
        void Activate();

        /// <summary>
        ///     Деактивирует объект.
        /// </summary>
        /// <code>
        /// Например: this.GameObject.SetActive(false);
        /// </code>
        void Deactivate();
    }
}
