namespace Assets.Scripts.Vision.Models.Scheduler.O7thTimeline
{
    using Assets.Scripts.ThinkingEngine.Models;
    using System.Collections.Generic;
    using UnityEngine;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfSchedulerO4thCommand = Assets.Scripts.Vision.Models.Scheduler.O4thCommands;
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

        public Model(ModelOfGame.Model gameModel)
        {
            this.GameModel = gameModel;
        }

        // - プロパティ

        internal ModelOfGame.Model GameModel { get; private set; }

        /// <summary>
        /// スケジュールに登録されている残りの項目
        /// 
        /// - 実行されると、 `ongoingItems` へ移動する
        /// </summary>

        List<ModelOfSchedulerO4thCommand.IModel> commands = new();

        internal List<ModelOfSchedulerO4thCommand.IModel> Commands => this.commands;

        /// <summary>
        /// タイム・ライン作成用カウンター
        /// 
        /// - プレイヤー別
        /// </summary>
        internal GameSeconds[] ScheduledTimesObj { get; private set; } = { GameSeconds.Zero, GameSeconds.Zero };

        // - メソッド

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="command">コマンド</param>
        internal void AddCommand(
            GameSeconds startObj,
            ModelOfThinkingEngineCommand.IModel command)
        {
            this.Commands.Add(ModelOfSchedulerO6thCommandMapping.Model.WrapCommand(
                startObj: startObj,
                command: command));
        }

        /// <summary>
        /// 追加
        /// 
        /// - タイムを自動的に付ける
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="command">コマンド引数</param>
        internal void AddWithinScheduler(Player playerObj, ModelOfThinkingEngineCommand.IModel command)
        {
            var commandOfScheduler = ModelOfSchedulerO6thCommandMapping.Model.WrapCommand(
                        startObj: this.ScheduledTimesObj[playerObj.AsInt],
                        command: command);

            this.Commands.Add(commandOfScheduler);

            // 次の時間
            this.ScheduledTimesObj[playerObj.AsInt] = new GameSeconds(
                this.ScheduledTimesObj[playerObj.AsInt].AsFloat + commandOfScheduler.TimeRangeObj.DurationObj.AsFloat);
        }

        internal void AddScheduleSeconds(Player playerObj, GameSeconds time)
        {
            this.ScheduledTimesObj[playerObj.AsInt] = new GameSeconds(this.ScheduledTimesObj[playerObj.AsInt].AsFloat + time.AsFloat);
        }

        internal ModelOfSchedulerO4thCommand.IModel GetCommandAt(int index)
        {
            return this.Commands[index];
        }

        internal int GetCountCommands()
        {
            return this.Commands.Count;
        }

        internal void RemoveAt(int index)
        {
            this.Commands.RemoveAt(index);
        }

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Vision.Models.Scheduler.SpanOfLerp.TimedGenerator.Simulator DebugWrite] commands.Count:{commands.Count}");
        }

        internal float LastSeconds() => Mathf.Max(this.ScheduledTimesObj[0].AsFloat, ScheduledTimesObj[1].AsFloat);
    }
}
