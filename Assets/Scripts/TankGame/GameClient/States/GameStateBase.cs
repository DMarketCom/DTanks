using SHLibrary.StateMachine;
using SHLibrary.Utils;
using TankGame.GameClient;

namespace Game.States
{
    public class GameStateBase : StateBase<GameSceneController, GameView>
    {
        protected GameModel Model { get { return Controller.Model; } }
        protected GameContext Context { get { return Controller.Context; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);
            View.BtnBack.onClick.AddListener(OnBackClick);
        }

        public override void Finish()
        {
            base.Finish();
            View.BtnBack.onClick.RemoveListener(OnBackClick);
        }

        protected override void OnBackClick()
        {
            base.OnBackClick();
            Controller.BackClicked.SafeRaise();
        }
    }
}
