using System;
using System.Collections.Generic;
using System.Linq;

using SHLibrary.Logging;

using Object = UnityEngine.Object;

namespace SHLibrary.ObjectPool
{
    public abstract class PoolBase<TPoolObject> : IPool
        where TPoolObject : UnityBehaviourBase, IPoolObject
    {
        private readonly IPoolStorage<TPoolObject> _poolStorage = new QueuePoolStorage<TPoolObject>();
        protected readonly List<TPoolObject> ActiveObjects = new List<TPoolObject>();

        internal virtual IPoolStorage<TPoolObject> PoolStorage
        {
            get { return _poolStorage; }
        }

        protected virtual int MaxPoolObjects
        {
            get { return int.MaxValue; }
        }

        public int ActiveObjectsCount
        {
            get { return ActiveObjects.Count; }
        }

        public Type PoolType
        {
            get { return typeof(TPoolObject); }
        }
        
        public void AddObjects(IEnumerable<IPoolObject> poolObjects)
        {
            foreach (IPoolObject poolObject in poolObjects)
            {
                var o = poolObject as TPoolObject;
                if (o != null)
                {
                    o.Deactivate();

                    poolObject.PoolObjectDestroyed += PoolObjectDestroyed;

                    PoolStorage.Add(o);
                }
            }
        }
        
        public void AddObjects(params IPoolObject[] poolObjects)
        {
            AddObjects(poolObjects.AsEnumerable());
        }

        public void Destroy()
        {
            ActiveObjects.Clear();
            PoolStorage.Clear();
        }

        public void Clear()
        {
            while (PoolStorage.Count > 0)
            {
                TPoolObject poolObject = PoolStorage.Get();
                if (poolObject != null)
                {
                    Object.Destroy(poolObject.gameObject);
                }
            }

            lock (ActiveObjects)
            {
                for (var i = 0; i < ActiveObjects.Count; i++)
                {
                    TPoolObject activeObject = ActiveObjects[i];
                    if (activeObject != null)
                    {
                        Object.Destroy(activeObject.gameObject);
                    }
                }

                ActiveObjects.Clear();
            }
        }

        public IPoolObject GetObject()
        {
            TPoolObject poolObject = null;
            if (PoolStorage.Count > 0)
            {
                while (poolObject == null && PoolStorage.Count > 0)
                {
                    poolObject = PoolStorage.Get();
                }
            }

            if (poolObject == null)
            {
                if (ActiveObjectsCount < MaxPoolObjects)
                {
                    poolObject = CreateObject();
                }
                else
                {
                    lock (ActiveObjects)
                    {
                        poolObject = ChooseActiveObjectWithMinimumPriority();
                        poolObject.PoolObjectDestroyed -= PoolObjectDestroyed;
                        poolObject.Deactivate();

                        ActiveObjects.Remove(poolObject);
                    }
                }
            }

            poolObject.PoolObjectDestroyed += PoolObjectDestroyed;

            poolObject.Activate();

            lock (ActiveObjects)
            {
                ActiveObjects.Add(poolObject);
            }

            return poolObject;
        }

        protected abstract TPoolObject CreateObject();

        protected virtual TPoolObject ChooseActiveObjectWithMinimumPriority()
        {
            return ActiveObjects[0];
        }

        private void PoolObjectDestroyed(IPoolObject poolObject)
        {
            poolObject.PoolObjectDestroyed -= PoolObjectDestroyed;
            poolObject.Deactivate();

            var o = poolObject as TPoolObject;
            if (ReferenceEquals(o, null) == false)
            {
                lock (ActiveObjects)
                {
                    ActiveObjects.Remove(o);
                }

                PoolStorage.Add(o);
            }
            else
            {
                DevLogger.Error(
                    string.Format("Pool object was not casted to type '{0}'", typeof(TPoolObject)),
                    LogChannel.Common);
            }
        }
    }
}
