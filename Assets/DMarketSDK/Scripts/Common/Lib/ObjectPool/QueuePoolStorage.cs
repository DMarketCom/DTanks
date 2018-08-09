using System.Collections.Generic;

namespace SHLibrary.ObjectPool
{
    internal class QueuePoolStorage<TPoolObject> : IPoolStorage<TPoolObject>
        where TPoolObject : UnityBehaviourBase, IPoolObject
    {
        private readonly Queue<TPoolObject> _pool = new Queue<TPoolObject>();

        public int Count
        {
            get { return _pool.Count; }
        }

        public void Add(TPoolObject poolObject)
        {
            _pool.Enqueue(poolObject);
        }

        public TPoolObject Get()
        {
            return _pool.Dequeue();
        }

        public void Clear()
        {
            _pool.Clear();
        }
    }
}
