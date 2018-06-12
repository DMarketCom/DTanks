using Lobby;
using SHLibrary.ObserverView;
using TankGame.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.Lobby
{
    public class LobbyView : ObserverViewBase<LobbyModel>
    {
        [SerializeField]
        private TMP_InputField _serverIPText;

        [SerializeField]
        private TMP_InputField _portText;

        [SerializeField]
        private TextMeshProUGUI _lobbyTypeText;

        public Button BtnIncreasePortNumber;
        public Button BtnDecreasePortNumber;
        public Button BtnConnect;
        public Button BtnBack;
        public WaitingForm WaitingForm;
        public MessageBoxForm MessageBoxForm;

        public string Port
        {
            get { return _portText.text; }
        }

        public string ServerIP
        {
            get { return _serverIPText.text; }
        }

        #region implemented abstract members of ObserverViewBase

        protected override void OnModelChanged()
        {
            _lobbyTypeText.text = Model.LobbyType.ToString();
            _serverIPText.text = Model.ServerIP;
            _portText.text = Model.Port.ToString();
        }

        #endregion
    }
}