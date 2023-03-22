namespace Assets.Scripts.ThinkingEngine.Models.CommandParameters
{
    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    internal class MoveCardsToHandFromPileModel : IModel
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="playerObj">ｎプレイヤー</param>
        /// <param name="numberOfCards">カードがｍ枚</param>
        internal MoveCardsToHandFromPileModel(Player playerObj, int numberOfCards)
        {
            PlayerObj = playerObj;
            NumberOfCards = numberOfCards;
        }

        // - プロパティ

        internal Player PlayerObj { get; private set; }
        internal int NumberOfCards { get; private set; }
    }
}
