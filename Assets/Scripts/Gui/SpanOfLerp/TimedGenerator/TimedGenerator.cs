namespace Assets.Scripts.Gui.SpanOfLerp.TimedGenerator
{
    using Assets.Scripts.Gui.SpanOfLerp.GeneratorGenerator;
    using Assets.Scripts.ThinkingEngine.CommandArgs;
    using GeneratorOfSpanOpLear = Assets.Scripts.Gui.SpanOfLerp.Generator;

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
        /// <param name="commandArgs">スパン・モデル</param>
        public TimedGenerator(float startSeconds, ICommandArgs commandArgs, ISpanGenerator spanGenerator)
        {
            this.StartSeconds = startSeconds;
            this.CommandArgs = commandArgs;
            this.SpanGenerator = spanGenerator;
        }

        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        public float StartSeconds { get; private set; }

        /// <summary>
        /// 持続時間（秒）
        /// </summary>
        public float Duration => CommandArgsAndDurationMapping.GetDurationBy(CommandArgs.GetType());

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        public float EndSeconds => StartSeconds + Duration;

        public ICommandArgs CommandArgs { get; private set; }

        public ISpanGenerator SpanGenerator { get; private set; }
    }
}
