namespace Assets.Scripts.Models.Timeline
{
    using Assets.Scripts.Models;
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

        List<ISpanModel> scheduledItemModels = new();

        /// <summary>
        /// 補間を実行中の項目
        /// 
        /// - 持続時間が切れると、消えていく
        /// </summary>
        List<ISpanModel> ongoingItems = new();

        internal List<ISpanModel> ScheduledItems
        {
            get
            {
                return this.scheduledItemModels;
            }
        }

        // - メソッド

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="spanModel">タイム・スパン</param>
        internal void Add(ISpanModel spanModel)
        {
            this.ScheduledItems.Add(spanModel);
        }

        /// <summary>
        /// コマンドを消化
        /// </summary>
        /// <param name="elapsedSeconds">ゲーム内消費時間（秒）</param>
        /// <param name="gameModelBuffer">ゲームの内部状態（編集可能）</param>
        /// <param name="gameViewModel">画面表示の状態（編集可能）</param>
        internal void OnEnter(float elapsedSeconds, GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            if (0 < this.ScheduledItems.Count)
            {
                var span = this.ScheduledItems[0];

                while (span.StartSeconds <= elapsedSeconds)
                {
                    // 持続中のコマンドへ移行したい
                    ongoingItems.Add(span);

                    // スケジュールから消化
                    this.ScheduledItems.RemoveAt(0);

                    // 初回実行
                    span.OnEnter(gameModelBuffer, gameViewModel);

                    if (0 < this.ScheduledItems.Count)
                    {
                        span = this.ScheduledItems[0];
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
        internal void Lerp(float elapsedSeconds)
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
                    float progress = (elapsedSeconds - ongoingItem.StartSeconds) / ongoingItem.Duration;
                    //// 超えることがある
                    //if (1.0f < progress)
                    //{
                    //    // 1.0 を超えるのもよくない
                    //    Debug.Log($"[Lerp] progress:{progress} (Over 1.0)");
                    //    progress = 1.0f;
                    //}

                    ongoingItem.Lerp(progress);
                }
            }
        }

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Models.Timeline.Model DebugWrite] timedItems.Count:{scheduledItemModels.Count} ongoingItems.Count:{ongoingItems.Count}");
        }
    }
}
