namespace Assets.Scripts.Simulators
{
    using Assets.Scripts.Gui.Models.Timeline;
    using System.Collections.Generic;
    using UnityEngine;
    using SimulatorsOfTimeline = Assets.Scripts.Simulators;

    /// <summary>
    /// シミュレーター
    /// 
    /// - 編集可能（Mutable）
    /// </summary>
    internal class ScheduleRegister
    {
        // - その他（生成）

        public ScheduleRegister()
        {
        }

        // - プロパティ

        /// <summary>
        /// スケジュールに登録されている残りの項目
        /// 
        /// - 実行されると、 `ongoingItems` へ移動する
        /// </summary>

        List<TimeSpan> timeSpans = new();

        internal List<TimeSpan> TimeSpans
        {
            get
            {
                return this.timeSpans;
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
        /// 追加
        /// 
        /// - タイムを自動的に付ける
        /// </summary>
        /// <param name="spanModel">タイム・スパン</param>
        internal void AddJustNow(float startSeconds, ISpanModel spanModel)
        {
            var timeSpan = new SimulatorsOfTimeline.TimeSpan(
                    startSeconds: startSeconds,
                    spanModel: spanModel,
                    spanView: Specification.SpawnViewFromModel(spanModel.GetType()));

            this.TimeSpans.Add(timeSpan);
        }

        /// <summary>
        /// 追加
        /// 
        /// - タイムを自動的に付ける
        /// </summary>
        /// <param name="spanModel">タイム・スパン</param>
        internal void AddWithinScheduler(int player, ISpanModel spanModel)
        {
            var timeSpan = new SimulatorsOfTimeline.TimeSpan(
                    startSeconds: this.ScheduledSeconds[player],
                    spanModel: spanModel,
                    spanView: Specification.SpawnViewFromModel(spanModel.GetType()));

            this.TimeSpans.Add(timeSpan);
            this.ScheduledSeconds[player] += timeSpan.Duration;
        }

        internal void AddScheduleSeconds(int player, float seconds)
        {
            this.ScheduledSeconds[player] += seconds;
        }

        internal TimeSpan GetItemAt(int index)
        {
            return this.TimeSpans[index];
        }

        internal int GetCountItems()
        {
            return this.TimeSpans.Count;
        }

        internal void RemoveAt(int index)
        {
            this.TimeSpans.RemoveAt(index);
        }

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Simulators.Simulator DebugWrite] timedItems.Count:{timeSpans.Count}");
        }
    }
}
