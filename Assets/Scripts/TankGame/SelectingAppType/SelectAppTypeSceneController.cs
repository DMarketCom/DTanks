using System;
using SHLibrary;
using UnityEngine.UI;
using UnityEngine;

namespace SelectAppType
{
    public class SelectAppTypeSceneController : UnityBehaviourBase
    {
        public event Action<AppType> ChooseAppType;
        public event Action PlayOffline;

        [SerializeField]
        private Button _btnClient;
        [SerializeField]
        private Button _btnServer;
        [SerializeField]
        private Button _btnOffline;

        public void Run()
        {
            var canRunServer = false;
#if UNITY_EDITOR || UNITY_STANDALONE
            canRunServer = true;
#endif
            _btnServer.interactable = canRunServer;
            _btnClient.onClick.AddListener(OnOnlineClick);
            _btnServer.onClick.AddListener(OnServelClick);
            _btnOffline.onClick.AddListener(OnOfflineClick);
        }

        public void Shutdown()
        {
            _btnClient.onClick.RemoveListener(OnOnlineClick);
            _btnServer.onClick.RemoveListener(OnServelClick);
            _btnOffline.onClick.RemoveListener(OnOfflineClick);
        }

        private void OnServelClick()
        {
            ChooseAppType.Invoke(AppType.Server);
        }

        private void OnOnlineClick()
        {
            ChooseAppType.Invoke(AppType.Client);
        }

        private void OnOfflineClick()
        {
            PlayOffline.Invoke();
        }
    }
}