namespace Assets.Scripts.Models.Timeline
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;

    /// <summary>
    /// タイム・スパン
    /// 
    /// - タイムライン上に配置されたもの
    /// </summary>
    interface ISpanModel
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
        float EndSeconds { get; }

        // - メソッド

        /// <summary>
        /// 開始時
        /// </summary>
        /// <param name="gameModelBuffer">ゲームの内部状態（編集可能）</param>
        /// <param name="gameViewModel">画面表示の状態（編集可能）</param>
        void OnEnter(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel);

        /// <summary>
        /// 持続中
        /// </summary>
        /// <param name="progress">進捗 0.0 ～ 1.0</param>
        void Lerp(float progress);

        /// <summary>
        /// 持続時間が切れたとき
        /// </summary>
        void OnLeave();
    }
}
