using Game;
using SHLibrary.StateMachine;

namespace TankGame.GameClient.Commands
{
    public class ShowGameOverPopUpCommand : ScheduledCommandBase<GameSceneController>
    {
        private GameView View { get { return Controller.View; } }

        public override void Start()
        {
            base.Start();

            ScheduledUpdate(1f);
            View.GameOverPopUp.ExitClicked += OnExitClicked;
            View.GameOverPopUp.RestartClicked += OnRestartClicked;
        }

        protected override void Finish()
        {
            base.Finish();

            View.GameOverPopUp.Hide();
            View.GameOverPopUp.ExitClicked -= OnExitClicked;
            View.GameOverPopUp.RestartClicked -= OnRestartClicked;
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();
            View.GameOverPopUp.Show();
        }

        private void OnRestartClicked()
        {
            Controller.RestartGame();
            Terminate(true);
        }

        private void OnExitClicked()
        {
            Controller.BackClicked.SafeRaise();
        }
    }
}