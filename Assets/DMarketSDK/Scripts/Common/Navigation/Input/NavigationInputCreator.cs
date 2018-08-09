using DMarketSDK.Common.Navigation.Keyboard;
using SHLibrary;

namespace DMarketSDK.Common.Navigation
{
    public class NavigationInputCreator : UnityBehaviourBase
    {
        protected override void Awake()
        {
            CreateNavigationInput();
        }

        private void CreateNavigationInput()
        {
#if UNITY_STANDALONE
            gameObject.AddComponent<KeyboardNavigationInput>();
#endif
        }
    }
}