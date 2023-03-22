namespace Assets.Scripts.Vision.Models.Scheduler.O7thTimeline
{
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using System.Collections.Generic;
    using UnityEngine;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfSchedulerO2ndTimedCommandArgs = Assets.Scripts.Vision.Models.Scheduler.O2ndTimedCommandArgs;
    using ModelOfSchedulerO5thTask = Assets.Scripts.Vision.Models.Scheduler.O5thTask;
    using ModelOfSchedulerO6thGameOperationMapping = Assets.Scripts.Vision.Models.Scheduler.O6thGameOperationMapping;

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

        internal List<ModelOfSchedulerO5thTask.Model> Tasks
        {
            get
            {
                return this.tasks;
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
        /// <param name="commandArg">コマンド引数</param>
        internal void AddJustNow(ModelOfSchedulerO2ndTimedCommandArgs.Model timedCommandArg)
        {
            var task = new ModelOfSchedulerO5thTask.Model(
                    startSeconds: GameModel.ElapsedSeconds,
                    args: timedCommandArg,
                    gameOperation: ModelOfSchedulerO6thGameOperationMapping.Model.NewGameOperationFromModel(timedCommandArg.CommandArg.GetType()));

            this.Tasks.Add(task);
        }

        /// <summary>
        /// 追加
        /// 
        /// - タイムを自動的に付ける
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="commandArg">コマンド引数</param>
        internal void AddWithinScheduler(Player playerObj, ICommandArg commandArg)
        {
            var task = new ModelOfSchedulerO5thTask.Model(
                    startSeconds: this.ScheduledSeconds[playerObj.AsInt],
                    args: new ModelOfSchedulerO2ndTimedCommandArgs.Model(commandArg),
                    gameOperation: ModelOfSchedulerO6thGameOperationMapping.Model.NewGameOperationFromModel(commandArg.GetType()));

            this.Tasks.Add(task);
            this.ScheduledSeconds[playerObj.AsInt] += task.Args.Duration;
        }

        internal void AddScheduleSeconds(Player playerObj, float seconds)
        {
            this.ScheduledSeconds[playerObj.AsInt] += seconds;
        }

        internal ModelOfSchedulerO5thTask.Model GetGameOperationSpanAt(int index)
        {
            return this.Tasks[index];
        }

        internal int GetCountGameOperationSpans()
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
