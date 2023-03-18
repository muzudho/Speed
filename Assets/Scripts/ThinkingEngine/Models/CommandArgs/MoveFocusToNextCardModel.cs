namespace Assets.Scripts.ThinkingEngine.Models.CommandArgs
{
    /// <summary>
    /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
    /// </summary>
    internal class MoveFocusToNextCardModel : ICommandArg
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="directionObj"></param>
        internal MoveFocusToNextCardModel(Player playerObj, PickingDirection directionObj)
        {
            this.PlayerObj = playerObj;
            this.DirectionObj = directionObj;
        }

        // - プロパティ

        internal Player PlayerObj { get; private set; }

        internal PickingDirection DirectionObj { get; private set; }
    }
}
