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
            // TODO ★ スレッド・セーフにしたい
            // キューに溜まっている分を全て消化
            int i = 0;
            while (i < this.ScheduledItems.Count)
            {
                var span = this.ScheduledItems[i];

                // まだ
                if (elapsedSeconds < span.StartSeconds)
                {
                    i++;
                    continue;
                }

                // 起動
                // ----
                Debug.Log($"[Assets.Scripts.Models.Timeline.Model OnEnter] タイム・スパン実行 span.StartSeconds:{span.StartSeconds} <= elapsedSeconds:{elapsedSeconds}");

                // スケジュールから除去
                this.ScheduledItems.RemoveAt(i);

                // 実行
                span.OnEnter(
                    gameModelBuffer,
                    gameViewModel,
                    setLaunchedSpanModel: setCardMovementModel);
            }
        }

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Models.Timeline.Model DebugWrite] timedItems.Count:{scheduledItemModels.Count}");
        }
    }
}
