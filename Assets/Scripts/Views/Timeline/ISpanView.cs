namespace Assets.Scripts.Views.Timeline
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;
    using SimulatorsOfTimeline = Assets.Scripts.Simulators.Timeline;

    /// <summary>
    /// タイム・スパン
    /// 
    /// - タイムライン上に配置されたもの
    /// </summary>
    interface ISpanView
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        ISpanView Spawn(SimulatorsOfTimeline.TimeSpan timeSpan);

        // - プロパティ

        /// <summary>
        /// タイム・スパン
        /// </summary>
        SimulatorsOfTimeline.TimeSpan TimeSpan { get; }

        // - メソッド

        /// <summary>
        /// 開始時
        /// </summary>
        /// <param name="gameModelBuffer">ゲームの内部状態（編集可能）</param>
        /// <param name="gameViewModel">画面表示の状態（編集可能）</param>
        void OnEnter(
            SimulatorsOfTimeline.TimeSpan timeSpan,
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<MovementViewModel> setMovementViewModel);
    }
}
