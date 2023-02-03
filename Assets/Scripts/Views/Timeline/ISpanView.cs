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
        // - プロパティ

        /// <summary>
        /// タイム・スパン
        /// </summary>
        TimeSpan TimeSpan { get; }

        // - メソッド

        /// <summary>
        /// 開始時
        /// </summary>
        /// <param name="gameModelBuffer">ゲームの内部状態（編集可能）</param>
        /// <param name="gameViewModel">画面表示の状態（編集可能）</param>
        void OnEnter(
            TimeSpan timeSpan,
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<CardMovementViewModel> setLaunchedSpanModel);
    }
}
