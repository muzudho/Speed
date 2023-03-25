namespace Assets.Scripts.Vision.Models.Input
{
    /// <summary>
    /// 入力の権利
    /// </summary>
    internal class Rights
    {
        /// <summary>
        /// コマンド実行の残り時間（秒）
        /// </summary>
        internal GameSeconds TimeOfRestObj { get; set; } = GameSeconds.Zero;
    }
}
