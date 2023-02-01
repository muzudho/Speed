namespace Assets.Scripts.Models.Timeline
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Commands;
    using Assets.Scripts.Views;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// タイムライン・モデル
    /// </summary>
    internal class Model
    {
        // - プロパティ

        /// <summary>
        /// スケジュールに登録されている残りの項目
        /// 
        /// - 実行されると、 `ongoingItems` へ移動する
        /// </summary>

        List<TimedItem> timedItems = new();

        /// <summary>
        /// 補間を実行中の項目
        /// 
        /// - 持続時間が切れると、消えていく
        /// </summary>
        List<TimedItem> ongoingItems = new();

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
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="command">コマンド</param>
        internal void Add(float seconds, float duration, ICommand command)
        {
            this.TimedItems.Add(new TimedItem(seconds, duration, command));
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
                var timedItem = timedItems[0];

                while (timedItem.StartSeconds <= elapsedSeconds)
                {
                    // 持続中のコマンドへ移行したい
                    ongoingItems.Add(timedItem);

                    // スケジュールから消化
                    timedItems.RemoveAt(0);

                    // 初回実行
                    timedItem.Command.DoIt(gameModelBuffer, gameViewModel);

                    if (0 < timedItems.Count)
                    {
                        timedItem = timedItems[0];
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// モーションの補間
        /// </summary>
        /// <param name="elapsedSeconds">ゲーム内消費時間（秒）</param>
        internal void Leap(float elapsedSeconds)
        {
            if (0 < ongoingItems.Count)
            {
                // 削除も行うので、逆順で
                for (int i = ongoingItems.Count - 1; 0 <= i; i--)
                {
                    var ongoingItem = ongoingItems[i];

                    // 期限切れ
                    if (ongoingItem.EndSeconds <= elapsedSeconds)
                    {
                        ongoingItem.OnLeave();

                        // 削除
                        ongoingItems.RemoveAt(i);
                        continue;
                    }

                    // TODO 持続中のコマンドの補間
                    ongoingItem.Leap(elapsedSeconds);
                }
            }
        }

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Models.Timeline.Model DebugWrite] timedItems.Count:{timedItems.Count} ongoingItems.Count:{ongoingItems.Count}");
        }
    }
}
