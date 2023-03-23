namespace Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan
{
    /// <summary>
    /// ゲーム時間範囲（単位：秒）
    /// </summary>
    internal class Range
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        public Range(
            GameSeconds startSeconds,
            GameSeconds duration)
        {
            this.StartTimeObj = startSeconds;
            this.DurationObj = duration;
            this.EndTimeObj = new GameSeconds(StartTimeObj.AsFloat + DurationObj.AsFloat);
        }

        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        public GameSeconds StartTimeObj { get; private set; }

        /// <summary>
        /// 持続時間（秒）
        /// </summary>
        public GameSeconds DurationObj { get; private set; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        public GameSeconds EndTimeObj { get; private set; }
    }
}
