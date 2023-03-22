namespace Assets.Scripts.ThinkingEngine.Models.Commands
{
    /// <summary>
    /// 間（ま）を設定
    /// </summary>
    internal class SetIdling : IModel
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
