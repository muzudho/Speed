namespace Assets.Scripts.Commands
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;
    using System.Collections.Generic;

    class DoingSimultaneously : ICommand
    {
        // - 生成
        internal DoingSimultaneously(List<ICommand> commands)
        {
            this.Commands = commands;
        }

        // - プロパティ

        List<ICommand> Commands { get; set; }

        // - メソッド

        public void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            foreach (var command in this.Commands)
            {
                command.DoIt(gameModelBuffer, gameViewModel);
            }
            this.Commands.Clear();
        }
    }
}
