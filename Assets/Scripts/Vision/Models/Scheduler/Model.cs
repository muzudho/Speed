namespace Assets.Scripts.Vision.Models.Scheduler
{
    using System.Collections.Generic;
    using UnityEngine;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfSchedulerO7thTimeline = Assets.Scripts.Vision.Models.Scheduler.O7thTimeline;

    /// <summary>
    /// スケジューラーのモデル
    /// 
    /// - 実行中の Lerp スパンを持ちます
    /// </summary>
    internal class Model
    {
        // - その他（生成）

        #region その他（生成）
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="gameModel"></param>
        public Model(ModelOfGame.Model gameModel)
        {
            // タイムラインは、ゲーム・モデルを持つ。
            this.Timeline = new ModelOfSchedulerO7thTimeline.Model(gameModel);

            this.CleanUp();
        }
        #endregion

        // - プロパティ

        #region プロパティ（タイムライン）
        /// <summary>
        /// タイムライン
        /// </summary>
        internal ModelOfSchedulerO7thTimeline.Model Timeline { get; }
        #endregion

        #region プロパティ（補間を実行中の項目）
        /// <summary>
        /// 補間を実行中の項目
        /// 
        /// - 持続時間が切れると、消えていく
        /// </summary>
        List<ModelOfSchedulerO1stTimelineSpan.IModel> ongoingSpans = new();
        #endregion

        // - メソッド

        #region メソッド（追加）
        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="additionOfSpans">追加の要素</param>
        internal void Add(List<ModelOfSchedulerO1stTimelineSpan.IModel> additionOfSpans)
        {
            foreach (var spanToLerp in additionOfSpans)
            {
                this.ongoingSpans.Add(spanToLerp);
            }

            // Debug.Log($"[Assets.Scripts.Vision.World.Views.Timeline.View Lerp] リープ ongoingCardMovementViews count:{ongoingCardMovementViews.Count}");
        }
        #endregion

        #region メソッド（この瞬間を描画）
        /// <summary>
        /// この瞬間を描画
        /// </summary>
        /// <param name="elapsedTime">ゲーム内消費時間（秒）</param>
        internal void Update(GameSeconds elapsedTime)
        {
            // TODO ★ スレッド・セーフにしたい
            // キューに溜まっている分を全て消化
            int i = 0;
            while (i < this.ongoingSpans.Count)
            {
                var ongoingSpan = ongoingSpans[i];
                float progress;

                // 期限切れ
                if (ongoingSpan.TimeRangeObj.EndObj.AsFloat <= elapsedTime.AsFloat)
                {
                    progress = 1.0f;

                    // 動作完了
                    //
                    // - 動作が完了する前に、次の動作を行うと、カードがどんどん沈んでいく、といったことが起こる。連打スパム対策は別途行ってください
                    ongoingSpan.Lerp(progress);

                    // （あれば）進行中の処理
                    if (ongoingSpan.OnProgressOrNull != null)
                    {
                        ongoingSpan.OnProgressOrNull(progress);
                    }

                    // リストから除去
                    ongoingSpans.RemoveAt(i);
                    continue;
                }

                // 進捗 0.0 ～ 1.0
                progress = (elapsedTime.AsFloat - ongoingSpan.TimeRangeObj.StartObj.AsFloat) / ongoingSpan.TimeRangeObj.DurationObj.AsFloat;

                // （あれば）進行中の処理
                if (ongoingSpan.OnProgressOrNull != null)
                {
                    ongoingSpan.OnProgressOrNull(progress);
                }

                // 補間
                ongoingSpan.Lerp(progress);

                i++;
            }
        }
        #endregion

        #region メソッド（初期化）
        internal void CleanUp()
        {
            this.Timeline.CleanUp();
            this.ongoingSpans.Clear();
        }
        #endregion

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Vision.World.Views.Timeline.View DebugWrite] ongoingItems.Count:{ongoingSpans.Count}");
        }

    }
}
