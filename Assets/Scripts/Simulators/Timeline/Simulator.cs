namespace Assets.Scripts.Simulators.Timeline
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views.Timeline;
    using Assets.Scripts.Views;
    using ModelsOfTimeline = Assets.Scripts.Models.Timeline;
    using UnityEngine;
    using Assets.Scripts.Models.Timeline;
    using Assets.Scripts.Models.Timeline.Spans;
    using System.Collections.Generic;
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
        /// コマンドを消化
        /// </summary>
        /// <param name="elapsedSeconds">ゲーム内消費時間（秒）</param>
        /// <param name="gameModelBuffer">ゲームの内部状態（編集可能）</param>
        /// <param name="gameViewModel">画面表示の状態（編集可能）</param>
        internal void OnEnter(
            float elapsedSeconds,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ViewMovement> setViewMovement)
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
                Debug.Log($"[Assets.Scripts.Models.Timeline.Model OnEnter] タイム・スパン実行 span.StartSeconds:{timeSpan.StartSeconds} <= elapsedSeconds:{elapsedSeconds}");

                // スケジュールから除去
                this.RemoveAt(i);

                // 実行
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
        internal void Add(int player, ISpanModel spanModel)
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
