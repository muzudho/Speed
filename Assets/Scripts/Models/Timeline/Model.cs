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
        internal void OnEnter(
            float elapsedSeconds,
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<CardMovementModel> setCardMovementModel)
        {
            if (0 < this.ScheduledItems.Count)
            {
                var span = this.ScheduledItems[0];

                while (span.StartSeconds <= elapsedSeconds)
                {
                    // TODO ★ 消す
                    // 持続中のコマンドへ移行したい
                    // setLaunchedSpanModel(span);

                    // スケジュールから消化
                    this.ScheduledItems.RemoveAt(0);

                    // 初回実行
                    span.OnEnter(
                        gameModelBuffer,
                        gameViewModel,
                        setLaunchedSpanModel: setCardMovementModel);

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

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Models.Timeline.Model DebugWrite] timedItems.Count:{scheduledItemModels.Count}");
        }
    }
}
