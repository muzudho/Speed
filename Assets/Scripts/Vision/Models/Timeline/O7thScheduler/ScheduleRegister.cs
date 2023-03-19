namespace Assets.Scripts.Vision.Models.Timeline.O7thScheduler
{
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using System.Collections.Generic;
    using UnityEngine;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfTimelineO2ndTimedCommandArgs = Assets.Scripts.Vision.Models.Timeline.O2ndTimedCommandArgs;
    using ModelOfTimelineO5thGameOperationSpan = Assets.Scripts.Vision.Models.Timeline.O5thGameOperationSpan;
    using ModelOfTimelineO6thGameOperationMapping = Assets.Scripts.Vision.Models.Timeline.O6thGameOperationMapping;

    /// <summary>
    /// シミュレーター
    /// 
    /// - 編集可能（Mutable）
    /// </summary>
    internal class ScheduleRegister
    {
        // - その他（生成）

        public ScheduleRegister(ModelOfGame.Default gameModel)
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

        List<ModelOfTimelineO5thGameOperationSpan.Model> gameOperationSpans = new();

        internal List<ModelOfTimelineO5thGameOperationSpan.Model> GameOperationSpans
        {
            get
            {
                return this.gameOperationSpans;
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
        internal void AddJustNow(ModelOfTimelineO2ndTimedCommandArgs.Model timedCommandArg)
        {
            var timedGenerator = new ModelOfTimelineO5thGameOperationSpan.Model(
                    startSeconds: GameModel.ElapsedSeconds,
                    timedCommandArg: timedCommandArg,
                    gameOperation: ModelOfTimelineO6thGameOperationMapping.Model.NewGameOperationFromModel(timedCommandArg.CommandArg.GetType()));

            this.GameOperationSpans.Add(timedGenerator);
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
            var timedGenerator = new ModelOfTimelineO5thGameOperationSpan.Model(
                    startSeconds: this.ScheduledSeconds[playerObj.AsInt],
                    timedCommandArg: new ModelOfTimelineO2ndTimedCommandArgs.Model(commandArg),
                    gameOperation: ModelOfTimelineO6thGameOperationMapping.Model.NewGameOperationFromModel(commandArg.GetType()));

            this.GameOperationSpans.Add(timedGenerator);
            this.ScheduledSeconds[playerObj.AsInt] += timedGenerator.TimedCommandArg.Duration;
        }

        internal void AddScheduleSeconds(Player playerObj, float seconds)
        {
            this.ScheduledSeconds[playerObj.AsInt] += seconds;
        }

        internal ModelOfTimelineO5thGameOperationSpan.Model GetItemAt(int index)
        {
            return this.GameOperationSpans[index];
        }

        internal int GetCountItems()
        {
            return this.GameOperationSpans.Count;
        }

        internal void RemoveAt(int index)
        {
            this.GameOperationSpans.RemoveAt(index);
        }

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Vision.Models.Timeline.SpanOfLerp.TimedGenerator.Simulator DebugWrite] timedItems.Count:{gameOperationSpans.Count}");
        }

        internal float LastSeconds() => Mathf.Max(this.ScheduledSeconds[0], ScheduledSeconds[1]);
    }
}
