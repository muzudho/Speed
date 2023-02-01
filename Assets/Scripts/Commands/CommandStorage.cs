namespace Assets.Scripts.Commands
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;
    using System.Collections.Generic;

    internal class CommandStorage
    {
        // - プロパティ
        List<ICommand> CommandList { get; set; } = new List<ICommand>();

        // - メソッド
        internal void Add(ICommand command)
        {
            this.CommandList.Add(command);
        }

        internal void Flush(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            foreach (var command in this.CommandList)
            {
                command.DoIt(gameModelBuffer, gameViewModel);
            }
            this.CommandList.Clear();
        }
    }
}
