namespace Assets.Scripts.Models.Commands
{
    using Assets.Scripts.Commands;
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;
    using System.Collections.Generic;

    internal class CommandTimeline
    {
        // - プロパティ

        float CurrentSeconds { get; set; }

        List<TimedCommand> timedCommands = new List<TimedCommand>();

        internal List<TimedCommand> TimedCommands
        {
            get
            {
                return this.timedCommands;
            }
        }

        // - メソッド

        internal void Add(float seconds, ICommand command)
        {
            this.TimedCommands.Add(new TimedCommand(seconds,command));
        }

        /// <summary>
        /// コマンドを１個取り出して実行
        /// 
        /// TODO ★ currentSeconds
        /// </summary>
        /// <param name="gameModelBuffer"></param>
        /// <param name="gameViewModel"></param>
        internal void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            if (0 < timedCommands.Count)
            {
                var timedCommandToRemove = timedCommands[0];
                timedCommands.RemoveAt(0);

                timedCommandToRemove.Command.DoIt(gameModelBuffer, gameViewModel);
            }
        }
    }
}
