namespace SHLibrary.ObserverView
{
    public abstract class ViewBase : UnityBehaviourBase
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}