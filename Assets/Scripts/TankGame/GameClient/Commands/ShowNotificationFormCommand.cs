using Game;
using SHLibrary.StateMachine;
using TankGame.Forms;

namespace TankGame.GameClient.Commands
{
    public class ShowNotificationFormCommand : ScheduledCommandBase<GameSceneController>
    {
        private readonly FormBase _targetForm;
        private readonly float _showDuration;
        private readonly float _animTime;

        public ShowNotificationFormCommand(FormBase targetForm, float showDuration, float animTime)
        {
            _targetForm = targetForm;
            _showDuration = showDuration;
            _animTime = animTime;
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