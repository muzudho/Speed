namespace Assets.Scripts.Vision.Models.Scheduler.O4thGameOperation
{
    using ModelOfSchedulerTimedCommandArgs = Assets.Scripts.Vision.Models.Scheduler.O2ndTimedCommandArgs;

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

        public ModelOfSchedulerTimedCommandArgs.Model Args { get; }

        public IModel GameOperation { get; }
    }
}
