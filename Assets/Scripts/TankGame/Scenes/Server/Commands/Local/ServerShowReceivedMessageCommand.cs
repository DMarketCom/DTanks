using System.Collections.Generic;
using System.Text;
using TankGame.Network.Messages;
using TankGame.Network.Server;
using TMPro;

namespace TankGame.GameServer.Commands.Local
{
    public class ServerShowReceivedMessageCommand : ServerCommandBase
    {
        private IGameServer GameServer { get { return Controller.GameServer; } }
        private IAppServer AppServer { get { return Controller.AppServer; } }

        private readonly TextMeshProUGUI _text;
        private readonly List<GameMsgType> _exceptedGameTypes;

        public ServerShowReceivedMessageCommand(TextMeshProUGUI text)
        {
            _text = text;
            _exceptedGameTypes = new List<GameMsgType>(new[] { GameMsgType.UnitPosition });
        }

        public override void Start()
        {
            base.Start();
            _text.text = string.Empty;
            AppServer.AppMsgReceived += OnAppMsgReceived;
            GameServer.GameMsgReceived += OnGameMessageReceived;
        }

        protected override void Finish()
        {
            base.Finish();
            AppServer.AppMsgReceived -= OnAppMsgReceived;
            GameServer.GameMsgReceived -= OnGameMessageReceived;
        }

        private void OnGameMessageReceived(GameMessageBase message)
        {
            if (!_exceptedGameTypes.Contains(message.Type))
            {
                ShowMsg(message.Type.ToString(), message.ClientId, "yellow");
            }
        }

        private void OnAppMsgReceived(AppMessageBase message)
        {
            ShowMsg(message.Type.ToString(), message.ConnectionId, "blue");
        }

        private void ShowMsg(string type, int conId, string color)
        {
            const int maxCount = 5;

            var userName = Model.IsUserSessionActive(conId) ?
                Model.GetUserNameByConnectionId(conId) : "connection id " + conId;

            var log = string.Format("{0} from {1}", type, userName);
            log = GetColorLog(log, color);
            var allLogs = _text.text.Split('\n');

            var newLogs = new StringBuilder();
            newLogs.Append("Message received:\n");
            newLogs.Append(log);
            newLogs.Append("\n");
            for (int i = 1; i < maxCount && i < allLogs.Length; i++)
            {
                newLogs.Append(allLogs[i] + "\n");
            }

            _text.text = newLogs.ToString();
        }

        private string GetColorLog(string message, string color)
        {
            return string.Format("<color={0}>{1}</color>", color, message);
        }
    }
}
