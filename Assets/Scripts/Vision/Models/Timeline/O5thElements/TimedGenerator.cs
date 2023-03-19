namespace Assets.Scripts.Vision.Models.Timeline.O5thElements
{
    using Assets.Scripts.Vision.Models.Timeline.O4thGameOperation;
    using ModelOfTimelineO2ndTimedCommandArgs = Assets.Scripts.Vision.Models.Timeline.O2ndTimedCommandArgs;
    using ModelOfTimelineO4thGameOperation = Assets.Scripts.Vision.Models.Timeline.O4thGameOperation;

    /// <summary>
    /// ゲーム内時間と、時間付きコマンド引数と、ゲーム内操作　を紐づけたもの
    /// </summary>
    internal class TimedGenerator : ITimedGenerator
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="timedCommandArg">スパン・モデル</param>
        /// <param name="gameOperation"></param>
        public TimedGenerator(
            float startSeconds,
            ModelOfTimelineO2ndTimedCommandArgs.Model timedCommandArg,
            ModelOfTimelineO4thGameOperation.IModel gameOperation)
        {
            this.StartSeconds = startSeconds;
            this.TimedCommandArg = timedCommandArg;
            this.GameOperation = gameOperation;
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

        public ModelOfTimelineO2ndTimedCommandArgs.Model TimedCommandArg { get; private set; }

        public ModelOfTimelineO4thGameOperation.IModel GameOperation { get; private set; }
    }
}
