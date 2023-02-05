namespace Assets.Scripts.Simulators.Timeline
{
    using Assets.Scripts.Gui.Models;
    using Assets.Scripts.Gui.Models.Timeline;
    using Assets.Scripts.Views.Timeline;
    using System.Collections.Generic;
    using UnityEngine;
    using SimulatorsOfTimeline = Assets.Scripts.Simulators.Timeline;

    /// <summary>
    /// シミュレーター
    /// 
    /// - 編集可能（Mutable）
    /// </summary>
    internal class Simulator
    {
        // - その他（生成）

        public Simulator()
        {
        }

        // - プロパティ

        /// <summary>
        /// スケジュールに登録されている残りの項目
        /// 
        /// - 実行されると、 `ongoingItems` へ移動する
        /// </summary>

        List<TimeSpan> scheduledItemModels = new();

        internal List<TimeSpan> ScheduledItems
        {
            get
            {
                return this.scheduledItemModels;
            }
        }

        /// <summary>
        /// タイム・ライン作成用カウンター
        /// 
        /// - プレイヤー別
        /// </summary>
        internal float[] ScheduledSeconds { get; private set; } = { 0.0f, 0.0f };

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        /// <param name="elapsedSeconds">ゲーム内消費時間（秒）</param>
        /// <param name="gameModelBuffer">ゲームの内部状態（編集可能）</param>
        /// <param name="gameViewModel">画面表示の状態（編集可能）</param>
        internal void OnEnter(
            float elapsedSeconds,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<SpanToLerp> setViewMovement)
        {
            // TODO ★ スレッド・セーフにしたい
            // キューに溜まっている分を全て消化
            int i = 0;
            while (i < this.GetCountItems())
            {
                var timeSpan = this.GetItemAt(i);

                // まだ
                if (elapsedSeconds < timeSpan.StartSeconds)
                {
                    i++;
                    continue;
                }

                // 起動
                // ----
                Debug.Log($"[Assets.Scripts.Gui.Models.Timeline.Model OnEnter] タイム・スパン実行 span.StartSeconds:{timeSpan.StartSeconds} <= elapsedSeconds:{elapsedSeconds}");

                // スケジュールから除去
                this.RemoveAt(i);

                // ゲーム画面の同期を始めます
                timeSpan.SpanView.OnEnter(
                    timeSpan,
                    gameModelBuffer,
                    setViewMovement: setViewMovement);
            }
        }

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="spanModel">タイム・スパン</param>
        internal void AddJustNow(float startSeconds, int player, ISpanModel spanModel)
        {
            var timeSpan = new SimulatorsOfTimeline.TimeSpan(
                    startSeconds: startSeconds,
                    spanModel: spanModel,
                    spanView: Specification.SpawnViewFromModel(spanModel.GetType()));

            this.ScheduledItems.Add(timeSpan);
        }

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="spanModel">タイム・スパン</param>
        internal void AddWithinScheduler(int player, ISpanModel spanModel)
        {
            var timeSpan = new SimulatorsOfTimeline.TimeSpan(
                    startSeconds: this.ScheduledSeconds[player],
                    spanModel: spanModel,
                    spanView: Specification.SpawnViewFromModel(spanModel.GetType()));

            this.ScheduledItems.Add(timeSpan);
            this.ScheduledSeconds[player] += timeSpan.Duration;
        }

        internal void AddScheduleSeconds(int player, float seconds)
        {
            this.ScheduledSeconds[player] += seconds;
        }

        internal TimeSpan GetItemAt(int index)
        {
            return this.ScheduledItems[index];
        }

        internal int GetCountItems()
        {
            return this.ScheduledItems.Count;
        }

        internal void RemoveAt(int index)
        {
            this.ScheduledItems.RemoveAt(index);
        }

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Simulators.Timeline.Simulator DebugWrite] timedItems.Count:{scheduledItemModels.Count}");
        }
    }
}
