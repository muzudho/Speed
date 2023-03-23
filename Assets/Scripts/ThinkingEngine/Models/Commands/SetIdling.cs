namespace Assets.Scripts.ThinkingEngine.Models.Commands
{
    using Assets.Scripts.Vision.Models;

    /// <summary>
    /// 間（ま）を設定
    /// </summary>
    internal class SetIdling : IModel
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="time">間</param>
        internal SetIdling(GameSeconds time)
        {
            TimeObj = time;
        }

        // - プロパティ

        internal GameSeconds TimeObj { get; private set; }
    }
}
