namespace Assets.Scripts.ThinkingEngine.DigitalCommands
{
    using Assets.Scripts.ThinkingEngine.Models;

    /// <summary>
    /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
    /// </summary>
    internal class MoveFocusToNextCard : IModel
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="directionObj"></param>
        internal MoveFocusToNextCard(Player playerObj, PickingDirection directionObj)
        {
            this.PlayerObj = playerObj;
            this.DirectionObj = directionObj;
        }

        // - プロパティ

        internal Player PlayerObj { get; private set; }

        internal PickingDirection DirectionObj { get; private set; }
    }
}
