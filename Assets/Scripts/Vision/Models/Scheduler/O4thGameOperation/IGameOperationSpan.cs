namespace Assets.Scripts.Vision.Models.Scheduler.O4thGameOperation
{
    using ModelOfSchedulerTimedCommandArgs = Assets.Scripts.Vision.Models.Scheduler.O2ndTimedCommandArgs;

    internal interface IGameOperationSpan
    {
        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        public float StartSeconds { get; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        public float EndSeconds => StartSeconds + this.TimedCommandArg.Duration;

        public ModelOfSchedulerTimedCommandArgs.Model TimedCommandArg { get; }

        public IModel GameOperation { get; }
    }
}
