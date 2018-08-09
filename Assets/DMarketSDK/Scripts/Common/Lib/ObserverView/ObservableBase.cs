using System.Collections.Generic;

namespace SHLibrary.ObserverView
{
    public class ObservableBase : IObservable
    {
        private readonly List<IObserver> _observers = new List<IObserver>();

        public void AddObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        public void SetChanges()
        {
            _observers.ForEach(observer => observer.OnChanged(this));
        }
    }
}
