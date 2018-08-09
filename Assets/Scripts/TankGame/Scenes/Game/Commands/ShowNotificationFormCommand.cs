using Game;
using SHLibrary.StateMachine;
using TankGame.UI.Forms;

namespace TankGame.GameClient.Commands
{
    public class ShowNotificationFormCommand : ScheduledCommandBase<GameSceneController>
    {
        private readonly FormBase _targetForm;
        private readonly float _showDuration;

        public ShowNotificationFormCommand(FormBase targetForm, float showDuration)
        {
            _targetForm = targetForm;
            _showDuration = showDuration;
        }

        public override void Start()
        {
            base.Start();

            _targetForm.Show();
            ScheduledUpdate(_showDuration);
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();

			_targetForm.Hide();
            Terminate(true);
        }
    }
}