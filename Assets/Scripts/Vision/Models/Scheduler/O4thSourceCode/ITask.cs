namespace Assets.Scripts.Vision.Models.Scheduler.O4thSourceCode
{
    using ModelOfSchedulerO2ndTaskParameters = Assets.Scripts.Vision.Models.Scheduler.O2ndTaskParameters;

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
        public float EndSeconds => StartSeconds + this.Args.Duration;

        public ModelOfSchedulerO2ndTaskParameters.Model Args { get; }

        public IModel SourceCode { get; }
    }
}
