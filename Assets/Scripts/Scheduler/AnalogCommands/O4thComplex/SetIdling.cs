namespace Assets.Scripts.Scheduler.AnalogCommands.O4thComplex
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
    /// なんにもしません
    /// </summary>
    class SetIdling : ItsAbstract
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startObj"></param>
        /// <param name="digitalCommand"></param>
        public SetIdling(
            GameSeconds startObj,
            ModelOfThinkingEngineDigitalCommands.IModel digitalCommand)
            : base(startObj, digitalCommand)
        {
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        public override void GenerateSpan(
            ModelOfGameBuffer.Model gameModelBuffer,
            ModelOfGameWriter.Model gameModelWriter,
            ModelOfInput.Init inputModel,
            ModelOfScheduler.Model schedulerModel,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimespan)
        {
            // なんにもしません
        }
    }
}
