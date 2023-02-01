namespace Assets.Scripts.Models.Timeline
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Commands;
    using Assets.Scripts.Views;
    using System.Collections.Generic;

    /// <summary>
    /// タイムライン・モデル
    /// </summary>
    internal class Model
    {
        // - プロパティ

        List<TimedItem> timedItems = new();

        internal List<TimedItem> TimedItems
        {
            get
            {
                return this.timedItems;
            }
        }

        // - メソッド

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="seconds">実行される時間（秒）</param>
        /// <param name="command">コマンド</param>
        internal void Add(float seconds, ICommand command)
        {
            this.TimedItems.Add(new TimedItem(seconds,command));
        }

        /// <summary>
        /// コマンドを消化
        /// </summary>
        /// <param name="elapsedSeconds">ゲーム内消費時間（秒）</param>
        /// <param name="gameModelBuffer">ゲームの内部状態（編集可能）</param>
        /// <param name="gameViewModel">画面表示の状態（編集可能）</param>
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
