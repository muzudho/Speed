namespace Assets.Scripts.Vision.Models.Scheduler.O4thSourceCode
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfThinkingEngineCommandParameter = Assets.Scripts.ThinkingEngine.Models.CommandParameters;

    /// <summary>
    /// なんにもしません
    /// </summary>
    class SetIdling : ItsAbstract
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override IModel NewThis()
        {
            return new SetIdling();
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        public override void Build(
            ITask task,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimelineSpan)
        {
            // なんにもしません
        }

        ModelOfThinkingEngineCommandParameter.SetIdling GetArg(ITask task)
        {
            return (ModelOfThinkingEngineCommandParameter.SetIdling)task.Args;
        }
    }
}
