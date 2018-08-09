using System.Collections.Generic;

namespace SHLibrary.StateMachine
{
    public class CommandQueue : CommandBase
    {
        private readonly List<CommandBase> _commands;
        private int _currentIndex;

        public CommandQueue(params CommandBase[] commands)
        {
            _commands = new List<CommandBase>();
            _commands.AddRange(commands);
        }

        public string Error { get; private set; }

        public override void Start()
        {
            base.Start();
            _currentIndex = 0;
            ExecuteNextCommand();
        }

        protected override void Finish()
        {
            base.Finish();
            _commands.Clear();
            Error = string.Empty;
        }

        private void ExecuteNextCommand()
        {
            if (_currentIndex == _commands.Count)
            {
                Terminate();
            }
            else
            {
                CommandBase command = _commands[_currentIndex];
                command.CommandFinished += OnCommandFinished;
                ApplyCommand(command);
            }
        }

        private void OnCommandFinished(CommandBase command)
        {
            command.CommandFinished -= OnCommandFinished;
            if (command.Result)
            {
                ExecuteNextCommand();
            }
            else
            {
                Error = string.Format("Could not execute command {0} with index {1}",
                    _commands[_currentIndex].GetType(), _currentIndex);
                Terminate(false);
            }
        }
    }
}
