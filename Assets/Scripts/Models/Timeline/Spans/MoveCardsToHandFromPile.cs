namespace Assets.Scripts.Models.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;

    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    class MoveCardsToHandFromPile : AbstractSpanModel
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="player">ｎプレイヤー</param>
        /// <param name="numberOfCards">カードがｍ枚</param>
        internal MoveCardsToHandFromPile(float startSeconds, float duration, int player, int numberOfCards)
            : base(startSeconds, duration)
        {
            Player = player;
            NumberOfCards = numberOfCards;
        }

        // - プロパティ

        int Player { get; set; }
        int NumberOfCards { get; set; }

        // - メソッド

        /// <summary>
        /// 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
        /// 
        /// - 画面上の場札は位置調整される
        /// </summary>
        public override void OnEnter(
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<CardMovementModel> setCardMovementModel)
        {
            // 手札の上の方からｎ枚抜いて、場札へ移動する
            var length = gameModelBuffer.IdOfCardsOfPlayersPile[Player].Count; // 手札の枚数

            if (NumberOfCards <= length)
            {
                // もし、場札が空っぽのところへ、手札を配ったのなら、先頭の場札をピックアップする
                if (gameModelBuffer.IndexOfFocusedCardOfPlayers[Player] == -1)
                {
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[Player] = 0;
                }

                var startIndex = length - NumberOfCards;

                gameModelBuffer.MoveCardsToHandFromPile(Player, startIndex, NumberOfCards);

                // 場札の位置の再調整（をしないと、手札から移動しない）
                GameModel gameModel = new GameModel(gameModelBuffer);
                gameViewModel.ArrangeHandCards(gameModel, Player, setCardMovementModel);
            }
        }
    }
}
