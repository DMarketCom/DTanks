namespace SHLibrary.ObserverView
{
    public interface IObserver
    {
        void OnChanged(IObservable model);
    }
}
