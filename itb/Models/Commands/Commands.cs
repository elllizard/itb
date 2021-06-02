using System.Collections.Generic;
using System.Linq;

namespace itb.Models.Commands
{
    public class Commands
    {
        private readonly List<Command> _commands;

        public Commands()
        {
            _commands = new List<Command>
            {
                new HomeCommand(),
                new ProfileCommand(),
                new ProfileAddUsernameCommand(),
                new ProfileAnalyzeMyProfileCommand(),
                new ProfileUpdateUsernameCommand(),
                new ProfileDeleteUsernameCommand(),
                new AnalyzeProfileCommand(),
                new CompareProfilesCommand()
            };
        }

        public Command GetCommandByName(string name)
        {
            Command _commandResult = null;
            foreach (Command _command in _commands.Where(command => command.NameEquals(name)))
            {
                _commandResult = _command;
            }

            return _commandResult;
        }

        public Command GetCommandByPath(string path)
        {
            Command _commandResult = null;
            foreach (Command _command in _commands.Where(command => command.PathEquals(path)))
            {
                _commandResult = _command;
            }

            return _commandResult;
        }
    }
}