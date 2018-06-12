using Networking.Client;
using Networking.Msg;

namespace Game.States.Online
{
    public class GameInitOnlineState : GameInitStateBase
    {
        private IGameClient Client { get { return Controller.Client; } }

        public override void Start(object[] args = null)
        {
            base.Start(args);
            View.MessageAlert.Init("Wait server response...", 0.2f);
            DestroyPreviousGameResources();
            SendConnectionRequest();
            Client.GameMsgReceived += OnGameMsgReceived;
            View.BtnBack.interactable = false;
        }

        private void DestroyPreviousGameResources()
        {
            foreach (var opponentTank in Controller.Opponents)
            {
                Controller.Context.DestroyTank(opponentTank);
            }
            Controller.Opponents.Clear();
            Context.PickUpManager.DestroyAllItems();
        }

        public override void Finish()
        {
            base.Finish();
            View.MessageAlert.Hide();
            Client.GameMsgReceived -= OnGameMsgReceived;
            View.BtnBack.interactable = true;
        }

        private void SendConnectionRequest()
        {
            Client.Send(new ConnectToBattleRequestMsg());
        }

        private void OnGameMsgReceived(GameMessageBase message)
        {
            if (message.Type == GameMsgType.ConnectToBattleAnswer)
            {
                var answer = message as ConnectToBattleAnswerMsg;
                if (answer.IsCanConnect)
                {
                    RespawnPlayer(answer.Player.Position, answer.Player.EquippedItemsTypes);
                    foreach (var dropItem in answer.DropedItems)
                    {
                        Context.PickUpManager.DropItem(dropItem.CatalogId, dropItem.WorldId,
                            dropItem.Pos);
                    }
                    ApplyState<GameIdleOnlineState>(answer.Oponents);
                }
                else
                {
                    ScheduledUpdate(1f, false);
                }

            }
        }

        protected override void OnScheduledUpdate()
        {
            base.OnScheduledUpdate();
            SendConnectionRequest();
        }
    }
}