namespace Assets.Scripts.Vision.Models.Timeline.O3rdBElements
{
    using ModelOfTimelineTimedCommandArgs = Assets.Scripts.Vision.Models.Timeline.TimedCommandArgs;

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

        public ISpanGenerator SpanGenerator { get; }
    }
}
