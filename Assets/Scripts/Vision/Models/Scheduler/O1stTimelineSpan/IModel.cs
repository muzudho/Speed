namespace Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan
{
    using System;

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
        /// 終了時の処理
        /// </summary>
        Action OnFinished { get; }

        // - メソッド

        /// <summary>
        /// 持続中
        /// </summary>
        /// <param name="progress">進捗 0.0 ～ 1.0</param>
        void Lerp(float progress);
    }
}
