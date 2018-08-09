namespace SHLibrary.ObserverView
{
    public interface IObservable
    {
        void AddObserver(IObserver observer);

        void RemoveObserver(IObserver observer);

        void SetChanges();
    }
}
