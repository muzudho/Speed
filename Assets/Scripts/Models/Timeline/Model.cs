namespace Assets.Scripts.Models.Timeline
{
    using Assets.Scripts.Commands;
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;
    using System.Collections.Generic;

    internal class Model
    {
        // - プロパティ

        float CurrentSeconds { get; set; }

        List<TimedItem> timedItems = new();

        internal List<TimedItem> TimedItems
        {
            get
            {
                return this.timedItems;
            }
        }

        // - メソッド

        internal void Add(float seconds, ICommand command)
        {
            this.TimedItems.Add(new TimedItem(seconds,command));
        }

        /// <summary>
        /// コマンドを消化
        /// </summary>
        /// <param name="elapsedSeconds">ゲーム内消費時間（秒）</param>
        /// <param name="gameModelBuffer"></param>
        /// <param name="gameViewModel"></param>
        internal void DoIt(float elapsedSeconds, GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            if (0 < timedItems.Count)
            {
                var timedCommand = timedItems[0];

                while (timedCommand.Seconds <= elapsedSeconds)
                {
                    // 消化
                    timedItems.RemoveAt(0);
                    timedCommand.Command.DoIt(gameModelBuffer, gameViewModel);

                    if (0 < timedItems.Count)
                    {
                        timedCommand = timedItems[0];
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
