namespace Assets.Scripts.ThinkingEngine.Models.CommandArgs
{
    /// <summary>
    /// 対局中か設定
    /// </summary>
    internal class SetGameActive : ICommandArg
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="isGameActive">対局中か</param>
        internal SetGameActive(bool isGameActive)
        {
            IsGameActive = isGameActive;
        }

        // - プロパティ

        internal bool IsGameActive { get; private set; }
    }
}
