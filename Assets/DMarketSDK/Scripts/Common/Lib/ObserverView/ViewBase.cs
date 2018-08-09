using UnityEngine;

namespace SHLibrary.ObserverView
{
    public abstract class ViewBase : MonoBehaviour
    {
        protected virtual void Start()
        {
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
