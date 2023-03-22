namespace Assets.Scripts.Vision.Models.Scheduler.O4thSourceCode
{
    using ModelOfCommandParameter = Assets.Scripts.ThinkingEngine.Models.CommandParameters;

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
        public float EndSeconds => StartSeconds + DurationMapping.GetDurationBy(this.Args.GetType());

        public ModelOfCommandParameter.IModel Args { get; }

        public IModel SourceCode { get; }
    }
}
