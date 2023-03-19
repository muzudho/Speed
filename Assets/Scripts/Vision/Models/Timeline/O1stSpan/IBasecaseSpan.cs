namespace Assets.Scripts.Vision.Models.Timeline.O1stSpan
{
    internal interface IBasecaseSpan
    {
        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        float StartSeconds { get; }

        /// <summary>
        /// 持続時間（秒）
        /// </summary>
        float Duration { get; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        /// <returns></returns>
        float EndSeconds { get; }

        // - メソッド

        /// <summary>
        /// 持続中
        /// </summary>
        /// <param name="progress">進捗 0.0 ～ 1.0</param>
        void Lerp(float progress);
    }
}
