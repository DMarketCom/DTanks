namespace DMarketSDK.Common.Navigation
{
    public static class NavigationRegister
    {
        private static readonly INavigationRegister _currentRegister;

        static NavigationRegister()
        {
            _currentRegister = new SortByAddTimeNavigationRegister();
        }

        public static void Add(INavigationElement element)
        {
            _currentRegister.Add(element);
        }

        public static void Remove(INavigationElement element)
        {
            _currentRegister.Remove(element);
        }

        public static bool IsCurrent(INavigationElement element)
        {
            return _currentRegister.IsCurrent(element);
        }
    }
}