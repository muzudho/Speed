namespace Assets.Scripts.Models.Timeline.Spans
{
    /// <summary>
    /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
    /// </summary>
    internal class MoveFocusToNextCardModel
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="player"></param>
        /// <param name="direction"></param>
        internal MoveFocusToNextCardModel(int player, int direction, LazyArgs.SetValue<int> setIndexOfNextFocusedHandCard)
        {
            this.Player = player;
            this.Direction = direction;
            this.SetIndexOfNextFocusedHandCard = setIndexOfNextFocusedHandCard;
        }

        // - プロパティ

        internal int Player { get; private set; }

        internal int Direction { get; private set; }

        internal LazyArgs.SetValue<int> SetIndexOfNextFocusedHandCard { get; private set; }
    }
}
