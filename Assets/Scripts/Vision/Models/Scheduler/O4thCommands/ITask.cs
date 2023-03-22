namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    internal interface ITask
    {
        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        public float StartSeconds { get; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        public float EndSeconds => StartSeconds + CommandDurationMapping.GetDurationBy(this.CommandOfThinkingEngine.GetType());

        public ModelOfThinkingEngineCommand.IModel CommandOfThinkingEngine { get; }

        public IModel CommandOfScheduler { get; }
    }
}
