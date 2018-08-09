namespace SHLibrary.ObjectPool
{
    public abstract class RandomPoolBase<TPoolObject> : PoolBase<TPoolObject>
        where TPoolObject : UnityBehaviourBase, IPoolObject
    {
        private readonly IPoolStorage<TPoolObject> _poolStorage = new RandomPoolStorage<TPoolObject>();

        internal override IPoolStorage<TPoolObject> PoolStorage
        {
            get { return _poolStorage; }
        }
    }
}
