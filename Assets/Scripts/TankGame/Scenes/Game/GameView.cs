using Game;
using TankGame.GameClient.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using SHLibrary.ObserverView;
using TankGame.UI.Forms;

namespace TankGame.GameClient
{
    public sealed class GameView : ObserverViewBase<GameModel>
    {
        [SerializeField]
        private Transform _notificationPanel;

        [SerializeField]
        private SimpleMessageForm _notificationFormPrefab;

        public int CountNotificationForm = 5;

        public Button BtnBack;
        public SimpleMessagePopUp MessageAlert;
        public GameOverPopUp GameOverPopUp;

        public GameObject MobileInputUI;

        private List<SimpleMessageForm> _notificationForms;

		public SimpleMessageForm CreateNotificationForm()
		{
            return _notificationForms.First(p => p.gameObject.activeSelf == false);
		}

        protected override void Start()
        {
            base.Start();
            CreateNotificationForms();

            #if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE || UNITY_WINRT || UNITY_WINRT_10_0 || UNITY_TIZEN
                MobileInputUI.SetActive(true);
            #endif
        }

        protected override void OnModelChanged()
        {
        }

        private void CreateNotificationForms()
        {
            _notificationForms = new List<SimpleMessageForm>();
            for (int i = 0; i < CountNotificationForm; i++)
            {
                _notificationForms.Add(Instantiate(_notificationFormPrefab, _notificationPanel));
            }
        }
    }
}