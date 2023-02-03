namespace Assets.Scripts.Views.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline;
    using Assets.Scripts.Views;

    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    class MoveCardsToHandFromPileView : AbstractSpanModel
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration1">持続時間（秒）　２段階モーションの１段目</param>
        /// <param name="duration2">持続時間（秒）　２段階モーションの２段目</param>
        /// <param name="player">ｎプレイヤー</param>
        /// <param name="numberOfCards">カードがｍ枚</param>
        internal MoveCardsToHandFromPileView(float startSeconds, float duration1, float duration2, int player, int numberOfCards)
            : base(startSeconds, duration1)
        {
            Player = player;
            NumberOfCards = numberOfCards;
        }

        // - プロパティ

        float Duration2 { get; set; }
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
                // 天辺から取っていく
                var startIndex = length - NumberOfCards;
                gameModelBuffer.MoveCardsToHandFromPile(Player, startIndex, NumberOfCards);

                // もし、場札が空っぽのところへ、手札を配ったのなら、先頭の場札をピックアップする
                if (gameModelBuffer.IndexOfFocusedCardOfPlayers[Player] == -1)
                {
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[Player] = 0;
                }

                // 場札の位置の再調整（をしないと、手札から移動しない）
                GameModel gameModel = new GameModel(gameModelBuffer);
                gameViewModel.ArrangeHandCards(
                    startSeconds: this.StartSeconds,
                    duration1: this.Duration,
                    duration2: this.Duration2,
                    gameModel: gameModel,
                    player: Player,
                    setCardMovementModel: setCardMovementModel);
            }
        }
    }
}
