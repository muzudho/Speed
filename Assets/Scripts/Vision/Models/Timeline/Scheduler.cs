namespace Assets.Scripts.Vision.Models.Timeline
{
    using System.Collections.Generic;
    using UnityEngine;
    using ModelOfTimelineO1stSpan = Assets.Scripts.Vision.Models.Timeline.O1stSpan;

    /// <summary>
    /// タイムラインのスケジューラー
    /// 
    /// - 実行中の Lerp スパンを持ちます
    /// </summary>
    internal class Scheduler
    {
        // - その他（生成）

        public Scheduler()
        {
        }

        // - プロパティ

        /// <summary>
        /// 補間を実行中の項目
        /// 
        /// - 持続時間が切れると、消えていく
        /// </summary>
        List<ModelOfTimelineO1stSpan.IBasecaseSpan> ongoingSpansToLerp = new();

        // - メソッド

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="additionOfSpansToLerp">追加の要素</param>
        internal void Add(List<ModelOfTimelineO1stSpan.IBasecaseSpan> additionOfSpansToLerp)
        {
            foreach (var spanToLerp in additionOfSpansToLerp)
            {
                this.ongoingSpansToLerp.Add(spanToLerp);
            }

            // Debug.Log($"[Assets.Scripts.Vision.World.Views.Timeline.View Lerp] リープ ongoingCardMovementViews count:{ongoingCardMovementViews.Count}");
        }

        /// <summary>
        /// この瞬間を描画
        /// </summary>
        /// <param name="elapsedSeconds">ゲーム内消費時間（秒）</param>
        internal void DrawThisMoment(float elapsedSeconds)
        {
            // TODO ★ スレッド・セーフにしたい
            // キューに溜まっている分を全て消化
            int i = 0;
            while (i < this.ongoingSpansToLerp.Count)
            {
                var ongoingCardMovementView = ongoingSpansToLerp[i];

                // 期限切れ
                if (ongoingCardMovementView.EndSeconds <= elapsedSeconds)
                {
                    // TODO ★★ 動作が完了する前に、次の動作を行うと、カードがどんどん沈んでいく、といったことが起こる。連打スパム対策が必要
                    // 動作完了
                    ongoingCardMovementView.Lerp(1.0f);

                    // リストから除去
                    ongoingSpansToLerp.RemoveAt(i);
                    continue;
                }

                // 進捗 0.0 ～ 1.0
                float progress = (elapsedSeconds - ongoingCardMovementView.StartSeconds) / ongoingCardMovementView.Duration;
                // 補間
                ongoingCardMovementView.Lerp(progress);

                i++;
            }
        }

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Vision.World.Views.Timeline.View DebugWrite] ongoingItems.Count:{ongoingSpansToLerp.Count}");
        }

    }
}
