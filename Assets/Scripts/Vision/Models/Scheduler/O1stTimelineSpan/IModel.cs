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
        /// 開始時間（秒）
        /// </summary>
        GameSeconds StartTimeObj { get; }

        /// <summary>
        /// 持続時間（秒）
        /// </summary>
        GameSeconds DurationObj { get; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        /// <returns></returns>
        GameSeconds EndTimeObj { get; }

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
