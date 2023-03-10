namespace Assets.Scripts.ThinkingEngine.Model.CommandArgs
{
    /// <summary>
    /// 間（ま）を設定
    /// </summary>
    internal class SetIdling : ICommandArg
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="seconds">間</param>
        internal SetIdling(float seconds)
        {
            Seconds = seconds;
        }

        // - プロパティ

        internal float Seconds { get; private set; }
    }
}
