using System.Collections.Generic;
using System.Linq;

namespace DMarketSDK.Common.Navigation
{
    public class SortBySiblingNavigationRegister : INavigationRegister
    {
        private readonly Dictionary<INavigationElement, int> _registerElements;

        private INavigationElement _lastSiblingElement;

        public SortBySiblingNavigationRegister()
        {
            _registerElements = new Dictionary<INavigationElement, int>();
            _lastSiblingElement = null;
        }

        public void Add(INavigationElement element)
        {
            if (_registerElements.ContainsKey(element))
            {
                _registerElements.Remove(element);
            }
            _registerElements.Add(element, 0);
            UpdateCurrentElement();
        }

        public void Remove(INavigationElement element)
        {
            if (_registerElements.ContainsKey(element))
            {
                _registerElements.Remove(element);
                UpdateCurrentElement();
            }
        }

        public bool IsCurrent(INavigationElement element)
        {
            return element.Equals(_lastSiblingElement);
        }

        public void ClearAll()
        {
            _registerElements.Clear();
            UpdateCurrentElement();
        }

        private int GetSiblingValue(INavigationElement element)
        {
            var elementTransform = element.RectTransform;
            var siblingValue = elementTransform.GetSiblingIndex();
            var navigationParent = elementTransform.parent;
            var parentCount = 0;
            while (navigationParent != null && parentCount < 5)
            {
                const int maxElementsInOneParent = 200;
                siblingValue -= maxElementsInOneParent;
                siblingValue += navigationParent.GetSiblingIndex();
                navigationParent = navigationParent.parent;
                parentCount++;
            }
            return siblingValue;
        }

        private void UpdateCurrentElement()
        {
            UpdateSiblingValues();
            _lastSiblingElement = null;
            foreach (var key in _registerElements.Keys)
            {
                if (_lastSiblingElement == null)
                {
                    _lastSiblingElement = key;
                }
                else if (_registerElements[key] > _registerElements[_lastSiblingElement])
                {
                    _lastSiblingElement = key;
                }
            }
        }

        private void UpdateSiblingValues()
        {
            var keys = _registerElements.Keys.ToArray();
            
            for (var i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                _registerElements[key] = GetSiblingValue(key);
            }
        }
    }
}