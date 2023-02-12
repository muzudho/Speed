namespace Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator
{
    using Assets.Scripts.Vision.World.SpanOfLerp.GeneratorGenerator;
    using GuiOfTimedCommandArgs = Assets.Scripts.Vision.World.TimedCommandArgs;

    /// <summary>
    /// コマンド引数と、スパン・ビューを紐づけます
    /// </summary>
    internal class TimedGenerator
    {
        // - その他（生成）

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="timedCommandArg">スパン・モデル</param>
        public TimedGenerator(float startSeconds, GuiOfTimedCommandArgs.Model timedCommandArg, ISpanGenerator spanGenerator)
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

        public GuiOfTimedCommandArgs.Model TimedCommandArg { get; private set; }

        public ISpanGenerator SpanGenerator { get; private set; }
    }
}
