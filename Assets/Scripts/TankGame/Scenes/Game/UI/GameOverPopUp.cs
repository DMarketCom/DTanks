using System;
using TankGame.UI.Forms;
using UnityEngine;
using UnityEngine.UI;

namespace TankGame.GameClient.UI
{
    public class GameOverPopUp : FormBase
    {
        public event Action ExitClicked;
        public event Action RestartClicked;

        [SerializeField]
        private Button _btnExit;

        [SerializeField]
        private Button _btnRestart;

        protected override void OnEnable()
        {
            base.OnEnable();
            _btnExit.onClick.AddListener(OnExitClicked);
            _btnRestart.onClick.AddListener(OnRestartClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _btnExit.onClick.RemoveListener(OnExitClicked);
            _btnRestart.onClick.RemoveListener(OnRestartClicked);
        }

        private void OnExitClicked()
        {
            ExitClicked.SafeRaise();
        }

        private void OnRestartClicked()
        {
            RestartClicked.SafeRaise();
        }
    }
}
