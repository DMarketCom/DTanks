using System.Collections.Generic;

namespace DMarketSDK.Common.Navigation
{
    public class SortByAddTimeNavigationRegister : INavigationRegister
    {
        private readonly List<INavigationElement> _elements
            = new List<INavigationElement>();
        
       public void Add(INavigationElement element)
        {
            if (_elements.Contains(element))
            {
                _elements.Remove(element);
            }
            _elements.Add(element);
        }

        public void Remove(INavigationElement element)
        {
            if (_elements.Contains(element))
            {
                _elements.Remove(element);
            }
        }

        public bool IsCurrent(INavigationElement element)
        {
            return _elements.Count > 0 &&
                   _elements[_elements.Count - 1].Equals(element);
        }

        public void ClearAll()
        {
            _elements.Clear();
        }
    }
}