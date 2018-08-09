using System;
using SHLibrary;
using UnityEngine.UI;
using UnityEngine;

namespace SelectAppType
{
    public class SelectAppTypeSceneController : UnityBehaviourBase
    {
        public event Action ChooseClient;
        public event Action ChooseServer;
        public event Action PlayOffline;

        [SerializeField]
        private Button _btnOnline;
        [SerializeField]
        private Button _btnOffline;
        [SerializeField]
        private Button _btnServer;

        public void Run()
        {
            _btnServer.enabled = !Application.isMobilePlatform;
            _btnOnline.onClick.AddListener(OnOnlineClick);
            _btnOffline.onClick.AddListener(OnOfflineClick);
            _btnServer.onClick.AddListener(OnServerClick);
        }

        public void Shutdown()
        {
            _btnOnline.onClick.RemoveListener(OnOnlineClick);
            _btnOffline.onClick.RemoveListener(OnOfflineClick);
            _btnServer.onClick.RemoveListener(OnServerClick);
        }

        private void OnServerClick()
        {
            ChooseServer.Invoke();
        }

        private void OnOnlineClick()
        {
            ChooseClient.Invoke();
        }

        private void OnOfflineClick()
        {
            PlayOffline.Invoke();
        }
    }
}