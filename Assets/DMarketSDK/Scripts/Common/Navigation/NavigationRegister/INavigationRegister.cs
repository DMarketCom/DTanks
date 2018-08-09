namespace DMarketSDK.Common.Navigation
{
    public interface INavigationRegister
    {
        void Add(INavigationElement element);

        void Remove(INavigationElement element);

        bool IsCurrent(INavigationElement element);

        void ClearAll();
    }
}