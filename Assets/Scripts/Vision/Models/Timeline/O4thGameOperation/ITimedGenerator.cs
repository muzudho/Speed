namespace Assets.Scripts.Vision.Models.Timeline.O4thGameOperation
{
    using ModelOfTimelineTimedCommandArgs = Assets.Scripts.Vision.Models.Timeline.O2ndTimedCommandArgs;

    internal interface ITimedGenerator
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

        public ModelOfTimelineTimedCommandArgs.Model TimedCommandArg { get; }

        public IModel GameOperation { get; }
    }
}
