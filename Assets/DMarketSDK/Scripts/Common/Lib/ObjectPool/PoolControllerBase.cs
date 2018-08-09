using System;
using System.Collections.Generic;

using SHLibrary.Logging;

namespace SHLibrary.ObjectPool
{
    public abstract class PoolControllerBase : UnityBehaviourBase
    {
        private readonly List<IPool> _pools = new List<IPool>();

        protected static TPoolObject GetObjectOfType<TPoolObject>(PoolControllerBase instance)
            where TPoolObject : IPoolObject
        {
            if (instance != null)
            {
                return instance.GetObject<TPoolObject>();
            }

            DevLogger.Error("Pool controller is not exists", LogChannel.Common);

            return default(TPoolObject);
        }

        protected void AddObjectsToPool(params IPoolObject[] poolObjects)
        {
            foreach (IPoolObject poolObject in poolObjects)
            {
                Type type = poolObject.GetType();
                foreach (IPool pool in _pools)
                {
                    if (pool.PoolType == type)
                    {
                        pool.AddObjects(poolObject);
                    }
                }
            }
        }

        protected virtual void OnPoolCreated()
        {
        }

        protected virtual void OnPoolDestroyed()
        {
        }

        protected override void Awake()
        {
            base.Awake();
            RegisterPools(_pools);
            OnPoolCreated();
        }

        protected abstract void RegisterPools(IList<IPool> pools);

        protected void AddPool(IPool pool)
        {
            _pools.Add(pool);
        }

        protected bool RemovePool(IPool pool)
        {
            return _pools.Remove(pool);
        }

        protected sealed override void OnDestroyObject()
        {
            foreach (IPool pool in _pools)
            {
                pool.Destroy();
            }

            OnPoolDestroyed();
        }

        private TPoolObject GetObject<TPoolObject>() where TPoolObject : IPoolObject
        {
            Type type = typeof(TPoolObject);
            foreach (IPool pool in _pools)
            {
                if (pool.PoolType == type)
                {
                    return pool.GetObject().To<TPoolObject>();
                }
            }

            DevLogger.Warning(
                string.Format("Cannot get object of '{0}' type because this pool does not exists", type));

            return default(TPoolObject);
        }
    }
}
