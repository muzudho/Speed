namespace Assets.Scripts.Vision.Models.Scheduler
{
    using System.Collections.Generic;
    using UnityEngine;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;

    /// <summary>
    /// スケジューラーのモデル
    /// 
    /// - 実行中の Lerp スパンを持ちます
    /// </summary>
    internal class Model
    {
        // - その他（生成）

        public Model()
        {
        }

        // - プロパティ

        /// <summary>
        /// 補間を実行中の項目
        /// 
        /// - 持続時間が切れると、消えていく
        /// </summary>
        List<ModelOfSchedulerO1stTimelineSpan.IModel> ongoingSpans = new();

        // - メソッド

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

        /// <summary>
        /// この瞬間を描画
        /// </summary>
        /// <param name="elapsedSeconds">ゲーム内消費時間（秒）</param>
        internal void DrawThisMoment(GameSeconds elapsedSeconds)
        {
            // TODO ★ スレッド・セーフにしたい
            // キューに溜まっている分を全て消化
            int i = 0;
            while (i < this.ongoingSpans.Count)
            {
                var ongoingSpan = ongoingSpans[i];

                // 期限切れ
                if (ongoingSpan.TimeRangeObj.EndObj.AsFloat <= elapsedSeconds.AsFloat)
                {
                    // TODO ★★ 動作が完了する前に、次の動作を行うと、カードがどんどん沈んでいく、といったことが起こる。連打スパム対策が必要
                    // 動作完了
                    ongoingSpan.Lerp(1.0f);

                    // （あれば）終了時の処理
                    if (ongoingSpan.OnFinished!=null)
                    {
                        ongoingSpan.OnFinished();
                    }

                    // リストから除去
                    ongoingSpans.RemoveAt(i);
                    continue;
                }

                // 進捗 0.0 ～ 1.0
                float progress = (elapsedSeconds.AsFloat - ongoingSpan.TimeRangeObj.StartObj.AsFloat) / ongoingSpan.TimeRangeObj.DurationObj.AsFloat;
                // 補間
                ongoingSpan.Lerp(progress);

                i++;
            }
        }

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Vision.World.Views.Timeline.View DebugWrite] ongoingItems.Count:{ongoingSpans.Count}");
        }

    }
}
