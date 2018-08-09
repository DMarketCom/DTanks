namespace SHLibrary.ObserverView
{
    public abstract class ObserverViewBase<T> : ViewBase, IObserver
        where T : IObservable
    {
        private bool _isNeedAdditionalUpdate;

        public T Model { private set; get; }

        public void OnChanged(IObservable model)
        {
            if (gameObject.activeInHierarchy)
            {
                OnModelChanged();
            }
            else
            {
                _isNeedAdditionalUpdate = true;
            }
        }

        public virtual void ApplyModel(T model)
        {
            if (Model != null)
            {
                Model.RemoveObserver(this);
            }
            Model = model;
            if (Model != null)
            {
                Model.AddObserver(this);
                OnModelChanged();
            }
        }

        protected abstract void OnModelChanged();

        protected virtual void OnDestroy()
        {
            if (Model != null)
            {
                Model.RemoveObserver(this);
            }
        }

        protected virtual void OnEnable()
        {
            if (_isNeedAdditionalUpdate)
            {
                if (Model != null)
                {
                    OnChanged(Model);
                }
            }
        }

        protected virtual void OnDisable()
        {
        }
    }
}
