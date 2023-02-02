namespace Assets.Scripts.Models.Timeline.Commands
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;

    /// <summary>
    /// コマンド
    /// </summary>
    interface ICommand
    {
        /// <summary>
        /// コマンド実行
        /// </summary>
        /// <param name="gameModelBuffer">ゲームの内部状態（編集可能）</param>
        /// <param name="gameViewModel">画面表示の状態（編集可能）</param>
        void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel);

        void Lerp(float elapsedSeconds);

        /// <summary>
        /// 持続時間が切れたとき
        /// </summary>
        void OnLeave();
    }
}
