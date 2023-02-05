namespace Assets.Scripts.Gui.Models.Timeline.Spans
{
    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    internal class MoveCardsToHandFromPileModel : ISpanModel
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="player">ｎプレイヤー</param>
        /// <param name="numberOfCards">カードがｍ枚</param>
        internal MoveCardsToHandFromPileModel(int player, int numberOfCards)
        {
            Player = player;
            NumberOfCards = numberOfCards;
        }

        // - プロパティ

        internal int Player { get; private set; }
        internal int NumberOfCards { get; private set; }
    }
}
