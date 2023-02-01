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

        /// <summary>
        /// コマンドを１個取り出して実行
        /// </summary>
        /// <param name="gameModelBuffer"></param>
        /// <param name="gameViewModel"></param>
        internal void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            if (0 < commands.Count)
            {
                var command = commands[0];
                commands.RemoveAt(0);

                command.DoIt(gameModelBuffer, gameViewModel);
            }
        }

        internal void Flush(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            foreach (var command in this.Commands)
            {
                command.DoIt(gameModelBuffer, gameViewModel);
            }
            this.Commands.Clear();
        }
    }
}
