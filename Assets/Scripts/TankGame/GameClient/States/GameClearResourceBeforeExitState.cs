namespace Game.States
{
    public class GameClearResourceBeforeExitState : GameStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);
            Controller.Player = null;
        }
    }
}
