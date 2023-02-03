namespace Assets.Scripts.Simulators.Timeline
{
    using Assets.Scripts.Models.Timeline;

    /// <summary>
    /// スパン・モデルと、スパン・ビューを紐づけます
    /// </summary>
    internal class TimeSpan
    {
        // - その他（生成）

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="spanModel">スパン・モデル</param>
        public TimeSpan(float startSeconds, ISpanModel spanModel)
        {
            this.StartSeconds = startSeconds;
            this.SpanModel = spanModel;
        }

        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        public float StartSeconds { get; private set; }

        /// <summary>
        /// 持続時間（秒）
        /// </summary>
        public float Duration
        {
            get
            {
                return Specification.GetDurationBy(SpanModel.GetType());
            }
        }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        public float EndSeconds => StartSeconds + Duration;

        public ISpanModel SpanModel { get; private set; }
    }
}
