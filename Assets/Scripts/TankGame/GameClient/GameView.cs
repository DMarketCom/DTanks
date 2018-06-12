using Game;
using SHLibrary.ObserverView;
using TankGame.Forms;
using TankGame.GameClient.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.GameClient
{
    public sealed class GameView : ObserverViewBase<GameModel>
    {
        [SerializeField]
        private SimpleMessageForm _notificationForm;

        public Button BtnBack;
        public SimpleMessagePopUp MessageAlert;
        public GameOverPopUp GameOverPopUp;

        public GameObject MobileInputUI;

        public SimpleMessageForm NotificationForm
        {
            get { return _notificationForm; }
        }

        protected override void Start()
        {
            base.Start();

            #if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE || UNITY_WINRT || UNITY_WINRT_10_0 || UNITY_TIZEN
                MobileInputUI.SetActive(true);
            #endif
        }

        protected override void OnModelChanged()
        {
        }
    }
}