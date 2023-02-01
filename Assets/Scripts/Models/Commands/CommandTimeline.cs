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
        /// コマンドを消化
        /// </summary>
        /// <param name="elapsedSeconds">ゲーム内消費時間（秒）</param>
        /// <param name="gameModelBuffer"></param>
        /// <param name="gameViewModel"></param>
        internal void DoIt(float elapsedSeconds, GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            if (0 < timedCommands.Count)
            {
                TimedCommand timedCommand = timedCommands[0];

                while (timedCommand.Seconds <= elapsedSeconds)
                {
                    // 消化
                    timedCommands.RemoveAt(0);
                    timedCommand.Command.DoIt(gameModelBuffer, gameViewModel);

                    if (0 < timedCommands.Count)
                    {
                        timedCommand = timedCommands[0];
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
