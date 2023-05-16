namespace Assets.Scripts.Scheduler.AnalogCommands.O4thComplexCommands
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.Vision.Models;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;
    using ModelOfGameWriter = Assets.Scripts.ThinkingEngine.Models.Game.Writer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfScheduler = Assets.Scripts.Scheduler.AnalogCommands;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan;
    using ModelOfThinkingEngineDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;

    /// <summary>
    /// ソースコードのようなもの
    /// 
    /// - 指定した時間と、そのとき実行されるコマンドのペア
    /// </summary>
    internal abstract class ItsAbstract : IModel
    {
        // - その他（生成）

        protected ItsAbstract(
            GameSeconds startTimeObj,
            ModelOfThinkingEngineDigitalCommands.IModel commandOfThinkingEngine)
        {
            this.TimeRangeObj = new ModelOfSchedulerO1stTimelineSpan.Range(startTimeObj, CommandDurationMapping.GetDurationBy(commandOfThinkingEngine.GetType()));
            this.CommandOfThinkingEngine = commandOfThinkingEngine;
        }

        // - プロパティ

        /// <summary>
        /// ゲーム時間範囲（単位：秒）
        /// </summary>
        public ModelOfSchedulerO1stTimelineSpan.Range TimeRangeObj { get; private set; }

        /// <summary>
        /// 思考エンジン用のコマンド
        /// </summary>
        public ModelOfThinkingEngineDigitalCommands.IModel CommandOfThinkingEngine { get; private set; }

        // - メソッド

        /// <summary>
        /// ビルド
        /// 
        /// - ゲーム画面の同期を始めます
        /// </summary>
        /// <param name="commandOfScheduler"></param>
        /// <param name="gameModelWriter"></param>
        /// <param name="setTimespan"></param>
        virtual public void GenerateSpan(
            ModelOfGameBuffer.Model gameModelBuffer,
            ModelOfGameWriter.Model gameModelWriter,
            ModelOfInput.Init inputModel,
            ModelOfScheduler.Model schedulerModel,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimespan)
        {
            // Ignored
        }
    }
}
