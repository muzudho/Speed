namespace Assets.Scripts.Vision.Models.Scheduler.O7thTimeline
{
    using Assets.Scripts.ThinkingEngine.Models;
    using System.Collections.Generic;
    using UnityEngine;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfSchedulerO5thTask = Assets.Scripts.Vision.Models.Scheduler.O5thTask;
    using ModelOfSchedulerO6thCommandMapping = Assets.Scripts.Vision.Models.Scheduler.O6thCommandMapping;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    /// <summary>
    /// タイムライン
    /// 
    /// - 編集可能（Mutable）
    /// </summary>
    internal class Model
    {
        // - その他（生成）

        public Model(ModelOfGame.Default gameModel)
        {
            this.GameModel = gameModel;
        }

        // - プロパティ

        internal ModelOfGame.Default GameModel { get; private set; }

        /// <summary>
        /// スケジュールに登録されている残りの項目
        /// 
        /// - 実行されると、 `ongoingItems` へ移動する
        /// </summary>

        List<ModelOfSchedulerO5thTask.Model> tasks = new();

        internal List<ModelOfSchedulerO5thTask.Model> Tasks=> this.tasks;

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
        /// <param name="commandArg">コマンド引数</param>
        internal void AddJustNow(ModelOfThinkingEngineCommand.IModel commandOfThinkingEngine)
        {
            var task = new ModelOfSchedulerO5thTask.Model(
                    startSeconds: GameModel.ElapsedSeconds,
                    commandOfThinkingEngine: commandOfThinkingEngine,
                    commandOfScheduler: ModelOfSchedulerO6thCommandMapping.Model.NewSourceCodeFromModel(commandOfThinkingEngine.GetType()));

            this.Tasks.Add(task);
        }

        /// <summary>
        /// 追加
        /// 
        /// - タイムを自動的に付ける
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="commandOfThinkingEngine">コマンド引数</param>
        internal void AddWithinScheduler(Player playerObj, ModelOfThinkingEngineCommand.IModel commandOfThinkingEngine)
        {
            var task = new ModelOfSchedulerO5thTask.Model(
                    startSeconds: this.ScheduledSeconds[playerObj.AsInt],
                    commandOfThinkingEngine: commandOfThinkingEngine,
                    commandOfScheduler: ModelOfSchedulerO6thCommandMapping.Model.NewSourceCodeFromModel(commandOfThinkingEngine.GetType()));

            this.Tasks.Add(task);
            this.ScheduledSeconds[playerObj.AsInt] += CommandDurationMapping.GetDurationBy(task.CommandOfThinkingEngine.GetType());
        }

        internal void AddScheduleSeconds(Player playerObj, float seconds)
        {
            this.ScheduledSeconds[playerObj.AsInt] += seconds;
        }

        internal ModelOfSchedulerO5thTask.Model GetTaskAt(int index)
        {
            return this.Tasks[index];
        }

        internal int GetCountTasks()
        {
            return this.Tasks.Count;
        }

        internal void RemoveAt(int index)
        {
            this.Tasks.RemoveAt(index);
        }

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Vision.Models.Scheduler.SpanOfLerp.TimedGenerator.Simulator DebugWrite] gameOperationSpans.Count:{tasks.Count}");
        }

        internal float LastSeconds() => Mathf.Max(this.ScheduledSeconds[0], ScheduledSeconds[1]);
    }
}
