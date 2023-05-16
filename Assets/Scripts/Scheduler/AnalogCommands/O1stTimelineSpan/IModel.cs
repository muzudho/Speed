namespace Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan
{
    using Assets.Scripts.Coding;

    /// <summary>
    /// タイムライン上のスパン要素
    /// </summary>
    internal interface IModel
    {
        // - プロパティ

        /// <summary>
        /// ゲーム時間範囲（単位：秒）
        /// </summary>
        Range TimeRangeObj { get; }

        /// <summary>
        /// 進行中の処理
        /// </summary>
        LazyArgs.SetValue<float> OnProgressOrNull { get; }

        // - メソッド

        /// <summary>
        /// 持続中
        /// </summary>
        /// <param name="progress">進捗 0.0 ～ 1.0</param>
        void Lerp(float progress);
    }
}
