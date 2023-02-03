namespace Assets.Scripts.Views.Timeline
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;

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
        ISpanView Spawn(TimeSpanView timeSpan);

        // - プロパティ

        /// <summary>
        /// タイム・スパン
        /// </summary>
        TimeSpanView TimeSpan { get; }

        // - メソッド

        /// <summary>
        /// 開始時
        /// </summary>
        /// <param name="gameModelBuffer">ゲームの内部状態（編集可能）</param>
        /// <param name="gameViewModel">画面表示の状態（編集可能）</param>
        void OnEnter(
            TimeSpanView timeSpan,
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<MovementViewModel> setCardMovementViewModel);
    }
}
