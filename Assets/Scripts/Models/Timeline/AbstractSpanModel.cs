namespace Assets.Scripts.Models.Timeline
{
    using Assets.Scripts.Views;

    /// <summary>
    /// タイム・スパン
    /// 
    /// - 指定した時間と、そのとき実行されるコマンドのペア
    /// </summary>
    internal abstract class AbstractSpanModel : ISpanModel
    {
        // - その他（生成）

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        public AbstractSpanModel(float startSeconds, float duration)
        {
            this.StartSeconds = startSeconds;
            this.Duration = duration;
        }

        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        public float StartSeconds { get; private set; }

        /// <summary>
        /// 持続時間（秒）
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        public float EndSeconds
        {
            get
            {
                return StartSeconds + Duration;
            }
        }

        // - メソッド

        virtual public void OnEnter(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            // Ignored
        }

        virtual public void Lerp(float progress)
        {
            // Ignored
        }

        /// <summary>
        /// 持続時間が切れたとき
        /// </summary>
        virtual public void OnLeave()
        {
            // Ignored
        }
    }
}
