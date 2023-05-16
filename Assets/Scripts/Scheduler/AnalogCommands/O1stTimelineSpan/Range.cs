namespace Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan
{
    using Assets.Scripts.Vision.Models;

    /// <summary>
    /// ゲーム時間範囲（単位：秒）
    /// </summary>
    internal class Range
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="start">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        public Range(
            GameSeconds start,
            GameSeconds duration)
        {
            this.StartObj = start;
            this.DurationObj = duration;
            this.EndObj = new GameSeconds(StartObj.AsFloat + DurationObj.AsFloat);
        }

        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        public GameSeconds StartObj { get; private set; }

        /// <summary>
        /// 持続時間（秒）
        /// </summary>
        public GameSeconds DurationObj { get; private set; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        public GameSeconds EndObj { get; private set; }
    }
}
