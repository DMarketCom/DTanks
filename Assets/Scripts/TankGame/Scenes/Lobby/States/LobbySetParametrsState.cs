using System;

namespace Lobby.States
{
    public class LobbySetParametrsState : LobbyStateBase
    {
        public override void Start(object[] args)
        {
            base.Start(args);
            View.BtnBack.gameObject.SetActive(true);
            View.BtnConnect.gameObject.SetActive(true);
            View.BtnBack.onClick.AddListener(OnBackClick);
            View.BtnConnect.onClick.AddListener(OnConnectClick);
            View.BtnDecreasePortNumber.interactable = true;
            View.BtnIncreasePortNumber.interactable = true;
            View.BtnDecreasePortNumber.onClick.AddListener(OnDecreasePortNumber);
            View.BtnIncreasePortNumber.onClick.AddListener(OnIncreasePortNumber);
        }

        public override void Finish()
        {
            base.Finish();
            View.BtnConnect.onClick.RemoveListener(OnConnectClick);
            View.BtnBack.onClick.RemoveListener(OnBackClick);
            View.BtnDecreasePortNumber.onClick.RemoveListener(OnDecreasePortNumber);
            View.BtnIncreasePortNumber.onClick.RemoveListener(OnIncreasePortNumber);
        }

        private void OnConnectClick()
        {
            UpdateModelFromView();
            ApplyState<LoobyWaitConnectionState>();
        }

        protected override void OnBackClick()
        {
            base.OnBackClick();
            Controller.LobbyBackEvent.SafeRaise();
        }

        private void OnDecreasePortNumber()
        {
            UpdateModelFromView();
            Model.Port--;
            Model.SetChanges();
        }

        private void OnIncreasePortNumber()
        {
            UpdateModelFromView();
            Model.Port++;
            Model.SetChanges();
        }

        private void UpdateModelFromView()
        {
            Model.Port = Int32.Parse(View.Port);
            Model.ServerIP = View.ServerIP;
        }
    }
}