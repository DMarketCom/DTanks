using System;

namespace SHLibrary.ObjectPool
{
    public interface IPool
    {
        int ActiveObjectsCount { get; }

        Type PoolType { get; }

        void Clear();

        void Destroy();

        void AddObjects(params IPoolObject[] poolObjects);

        IPoolObject GetObject();
    }
}
