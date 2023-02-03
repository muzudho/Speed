namespace Assets.Scripts.Views.Timeline
{
    using Assets.Scripts.Models.Timeline;
    using Assets.Scripts.Simulators.Timeline;

    internal class TimeSpanView
    {
        // - その他（生成）

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="spanModel">スパン・モデル</param>
        public TimeSpanView(float startSeconds, ISpanModel spanModel)
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

        public ISpanModel SpanModel { get; private set; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        public float EndSeconds => StartSeconds + Duration;
    }
}
