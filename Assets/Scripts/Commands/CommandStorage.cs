namespace Assets.Scripts.Commands
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;
    using System.Collections.Generic;

    internal class CommandStorage
    {
        // - プロパティ

        List<ICommand> commands = new List<ICommand>();
        internal List<ICommand> Commands
        {
            get
            {
                return this.commands;
            }
        }

        // - メソッド
        internal void Add(ICommand command)
        {
            this.Commands.Add(command);
        }

        internal void Clear()
        {
            this.Commands.Clear();
        }

        internal void Flush(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel, LazyArgs.Action afterStep)
        {
            foreach (var command in this.Commands)
            {
                command.DoIt(gameModelBuffer, gameViewModel);
                afterStep();
            }
            this.Commands.Clear();
        }
    }
}
