namespace Assets.Scripts.ThinkingEngine.Models.Commands
{
    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    internal class MoveCardsToHandFromPile : IModel
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="playerObj">ｎプレイヤー</param>
        /// <param name="numberOfCards">カードがｍ枚</param>
        internal MoveCardsToHandFromPile(Player playerObj, int numberOfCards)
        {
            PlayerObj = playerObj;
            NumberOfCards = numberOfCards;
        }

        // - プロパティ

        internal Player PlayerObj { get; private set; }
        internal int NumberOfCards { get; private set; }
    }
}
