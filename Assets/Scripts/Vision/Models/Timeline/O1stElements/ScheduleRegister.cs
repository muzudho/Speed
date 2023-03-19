namespace Assets.Scripts.Vision.Models.Timeline.O1stElements
{
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using System.Collections.Generic;
    using UnityEngine;
    using GuiOfTimedCommandArgs = Assets.Scripts.Vision.Models.Timeline.TimedCommandArgs;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using TimedGeneratorOfSpanOfLearp = Assets.Scripts.Vision.Models.Timeline.O1stElements;

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

        List<TimedGenerator> timedGenerators = new();

        internal List<TimedGenerator> TimedGenerators
        {
            get
            {
                return this.timedGenerators;
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
        internal void AddJustNow(GuiOfTimedCommandArgs.Model timedCommandArg)
        {
            var timedGenerator = new TimedGeneratorOfSpanOfLearp.TimedGenerator(
                    startSeconds: GameModel.ElapsedSeconds,
                    timedCommandArg: timedCommandArg,
                    spanGenerator: TimedGeneratorOfSpanOfLearp.Mapping.SpawnViewFromModel(timedCommandArg.CommandArg.GetType()));

            this.TimedGenerators.Add(timedGenerator);
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
            var timedGenerator = new TimedGeneratorOfSpanOfLearp.TimedGenerator(
                    startSeconds: this.ScheduledSeconds[playerObj.AsInt],
                    timedCommandArg: new GuiOfTimedCommandArgs.Model(commandArg),
                    spanGenerator: TimedGeneratorOfSpanOfLearp.Mapping.SpawnViewFromModel(commandArg.GetType()));

            this.TimedGenerators.Add(timedGenerator);
            this.ScheduledSeconds[playerObj.AsInt] += timedGenerator.TimedCommandArg.Duration;
        }

        internal void AddScheduleSeconds(Player playerObj, float seconds)
        {
            this.ScheduledSeconds[playerObj.AsInt] += seconds;
        }

        internal TimedGenerator GetItemAt(int index)
        {
            return this.TimedGenerators[index];
        }

        internal int GetCountItems()
        {
            return this.TimedGenerators.Count;
        }

        internal void RemoveAt(int index)
        {
            this.TimedGenerators.RemoveAt(index);
        }

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Vision.Models.Timeline.SpanOfLerp.TimedGenerator.Simulator DebugWrite] timedItems.Count:{timedGenerators.Count}");
        }

        internal float LastSeconds() => Mathf.Max(this.ScheduledSeconds[0], ScheduledSeconds[1]);
    }
}
