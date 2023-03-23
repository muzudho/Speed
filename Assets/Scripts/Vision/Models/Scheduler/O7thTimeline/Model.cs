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

        internal List<ModelOfSchedulerO5thTask.Model> Tasks => this.tasks;

        /// <summary>
        /// タイム・ライン作成用カウンター
        /// 
        /// - プレイヤー別
        /// </summary>
        internal GameSeconds[] ScheduledTimesObj { get; private set; } = { GameSeconds.Zero, GameSeconds.Zero };

        // - メソッド

        /// <summary>
        /// 追加
        /// 
        /// - タイムを自動的に付ける
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        internal void AddJustNow(ModelOfThinkingEngineCommand.IModel command)
        {
            var task = new ModelOfSchedulerO5thTask.Model(
                    commandOfScheduler: ModelOfSchedulerO6thCommandMapping.Model.WrapCommand(
                        startObj: this.GameModel.ElapsedSeconds,
                        command: command));

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
                    commandOfScheduler: ModelOfSchedulerO6thCommandMapping.Model.WrapCommand(
                        startObj: this.ScheduledTimesObj[playerObj.AsInt],
                        command: commandOfThinkingEngine));

            this.Tasks.Add(task);

            var a = this.ScheduledTimesObj[playerObj.AsInt].AsFloat;
            var key = task.CommandOfScheduler.CommandOfThinkingEngine.GetType();
            var b = CommandDurationMapping.GetDurationBy(key).AsFloat;
            this.ScheduledTimesObj[playerObj.AsInt] = new GameSeconds(a + b);
        }

        internal void AddScheduleSeconds(Player playerObj, GameSeconds time)
        {
            this.ScheduledTimesObj[playerObj.AsInt] = new GameSeconds(this.ScheduledTimesObj[playerObj.AsInt].AsFloat + time.AsFloat);
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

        internal float LastSeconds() => Mathf.Max(this.ScheduledTimesObj[0].AsFloat, ScheduledTimesObj[1].AsFloat);
    }
}
