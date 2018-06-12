using SHLibrary.Utils;

namespace Lobby.States
{
    public class LoobyWaitConnectionState : LobbyStateBase
    {
        public override void Start(object[] args)
        {
            base.Start(args);
            View.BtnBack.gameObject.SetActive(true);
            View.BtnConnect.gameObject.SetActive(false);
            View.BtnBack.onClick.AddListener(OnBackClick);
            View.BtnDecreasePortNumber.interactable = false;
            View.BtnIncreasePortNumber.interactable = false;

            View.WaitingForm.Show();

            const float kReconnectTime = 3f;
            ScheduledUpdate(kReconnectTime, true);
            OnScheduledUpdate();
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();
            Controller.LobbyWaitConnection.SafeRaise(Model.ServerIP, Model.Port);
        }

        public override void Finish()
        {
            base.Finish();
            View.BtnConnect.onClick.RemoveListener(OnBackClick);
            View.WaitingForm.Show();
        }

        protected override void OnBackClick()
        {
            base.OnBackClick();
            ApplyState<LobbySetParametrsState>();
        }
    }
}