namespace Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator
{
    using Assets.Scripts.ThinkingEngine.Model;
    using Assets.Scripts.ThinkingEngine.Model.CommandArgs;
    using System.Collections.Generic;
    using UnityEngine;
    using GuiOfTimedCommandArgs = Assets.Scripts.Vision.World.TimedCommandArgs;
    using TimedGeneratorOfSpanOfLearp = Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator;

    /// <summary>
    /// シミュレーター
    /// 
    /// - 編集可能（Mutable）
    /// </summary>
    internal class ScheduleRegister
    {
        // - その他（生成）

        public ScheduleRegister(GameModel gameModel)
        {
            this.GameModel = gameModel;
        }

        // - プロパティ

        internal GameModel GameModel { get; private set; }

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
        /// <param name="commandArg">コマンド引数</param>
        internal void AddWithinScheduler(int player, ICommandArg commandArg)
        {
            var timedGenerator = new TimedGeneratorOfSpanOfLearp.TimedGenerator(
                    startSeconds: this.ScheduledSeconds[player],
                    timedCommandArg: new GuiOfTimedCommandArgs.Model(commandArg),
                    spanGenerator: TimedGeneratorOfSpanOfLearp.Mapping.SpawnViewFromModel(commandArg.GetType()));

            this.TimedGenerators.Add(timedGenerator);
            this.ScheduledSeconds[player] += timedGenerator.TimedCommandArg.Duration;
        }

        internal void AddScheduleSeconds(int player, float seconds)
        {
            this.ScheduledSeconds[player] += seconds;
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
            Debug.Log($"[Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator.Simulator DebugWrite] timedItems.Count:{timedGenerators.Count}");
        }
    }
}
