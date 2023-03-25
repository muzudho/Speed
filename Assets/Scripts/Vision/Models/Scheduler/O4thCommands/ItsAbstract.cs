namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

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
            ModelOfThinkingEngineCommand.IModel commandOfThinkingEngine)
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
        public ModelOfThinkingEngineCommand.IModel CommandOfThinkingEngine { get; private set; }

        // - メソッド

        /// <summary>
        /// ビルド
        /// 
        /// - ゲーム画面の同期を始めます
        /// </summary>
        /// <param name="commandOfScheduler"></param>
        /// <param name="gameModelBuffer"></param>
        /// <param name="setTimelineSpan"></param>
        virtual public void GenerateSpan(
            GameModelBuffer gameModelBuffer,
            ModelOfInput.Init inputModel,
            ModelOfScheduler.Model schedulerModel,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimelineSpan)
        {
            // Ignored
        }
    }
}
