namespace Assets.Scripts.Vision.Models.Timeline.O1stElements
{
    using Assets.Scripts.Vision.Models.Timeline.O3rdBElements;
    using ModelOfTimelineO3rdElement = Assets.Scripts.Vision.Models.Timeline.O3rdBElements;
    using ModelOfTimelineTimedCommandArgs = Assets.Scripts.Vision.Models.Timeline.TimedCommandArgs;

    /// <summary>
    /// ゲーム内時間と、時間付きコマンド引数と、スパン生成器を紐づけたもの
    /// </summary>
    internal class TimedGenerator : ITimedGenerator
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="timedCommandArg">スパン・モデル</param>
        /// <param name="spanGenerator"></param>
        public TimedGenerator(
            float startSeconds,
            ModelOfTimelineTimedCommandArgs.Model timedCommandArg,
            ModelOfTimelineO3rdElement.ISpanGenerator spanGenerator)
        {
            this.StartSeconds = startSeconds;
            this.TimedCommandArg = timedCommandArg;
            this.SpanGenerator = spanGenerator;
        }

        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        public float StartSeconds { get; private set; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        public float EndSeconds => StartSeconds + this.TimedCommandArg.Duration;

        public ModelOfTimelineTimedCommandArgs.Model TimedCommandArg { get; private set; }

        public ModelOfTimelineO3rdElement.ISpanGenerator SpanGenerator { get; private set; }
    }
}
