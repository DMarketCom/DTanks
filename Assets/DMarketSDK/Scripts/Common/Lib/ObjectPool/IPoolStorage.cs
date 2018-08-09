namespace SHLibrary.ObjectPool
{
    internal interface IPoolStorage<TPoolObject>
        where TPoolObject : UnityBehaviourBase, IPoolObject
    {
        int Count { get; }

        void Add(TPoolObject poolObject);

        TPoolObject Get();

        void Clear();
    }
}
