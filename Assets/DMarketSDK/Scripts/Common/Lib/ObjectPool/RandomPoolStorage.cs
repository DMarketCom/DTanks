using System;
using System.Collections.Generic;

namespace SHLibrary.ObjectPool
{
    internal class RandomPoolStorage<TPoolObject> : IPoolStorage<TPoolObject>
        where TPoolObject : UnityBehaviourBase, IPoolObject
    {
        private readonly List<TPoolObject> _pool = new List<TPoolObject>();

        private readonly Random _random = new Random();

        public int Count
        {
            get { return _pool.Count; }
        }

        public void Add(TPoolObject poolObject)
        {
            _pool.Add(poolObject);
        }

        public TPoolObject Get()
        {
            if (_pool.Count <= 0)
            {
                throw new InvalidOperationException("There is no pool objects.");
            }

            int index = _random.Next(0, _pool.Count);
            TPoolObject result = _pool[index];
            _pool.RemoveAt(index);

            return result;
        }

        public void Clear()
        {
            _pool.Clear();
        }
    }
}
