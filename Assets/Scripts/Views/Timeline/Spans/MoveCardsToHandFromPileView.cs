namespace Assets.Scripts.Views.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Spans;
    using Assets.Scripts.Views;

    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    class MoveCardsToHandFromPileView : AbstractSpanView
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override ISpanView Spawn(TimeSpanView timeSpan)
        {
            return new MoveCardsToHandFromPileView(timeSpan);
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="timeSpan">タイム・スパン</param>
        internal MoveCardsToHandFromPileView(TimeSpanView timeSpan)
            : base(timeSpan)
        {
        }

        // - プロパティ

        MoveCardsToHandFromPileModel Model
        {
            get
            {
                return (MoveCardsToHandFromPileModel)this.TimeSpan.SpanModel;
            }
        }

        // - メソッド

        /// <summary>
        /// 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
        /// 
        /// - 画面上の場札は位置調整される
        /// </summary>
        public override void OnEnter(
            TimeSpanView timeSpan,
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<CardMovementViewModel> setCardMovementViewModel)
        {
            var length = gameModelBuffer.IdOfCardsOfPlayersPile[this.Model.Player].Count; // 手札の枚数

            if (this.Model.NumberOfCards <= length)
            {
                // 天辺から取っていく
                var startIndex = length - this.Model.NumberOfCards;
                gameModelBuffer.MoveCardsToHandFromPile(this.Model.Player, startIndex, this.Model.NumberOfCards);

                // もし、場札が空っぽのところへ、手札を配ったのなら、先頭の場札をピックアップする
                if (gameModelBuffer.IndexOfFocusedCardOfPlayers[this.Model.Player] == -1)
                {
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[this.Model.Player] = 0;
                }

                // 場札の位置の再調整（をしないと、手札から移動しない）
                GameModel gameModel = new GameModel(gameModelBuffer);
                gameViewModel.ArrangeHandCards(
                    startSeconds: timeSpan.StartSeconds,
                    duration1: timeSpan.Duration / 2.0f,
                    duration2: timeSpan.Duration / 2.0f,
                    gameModel: gameModel,
                    player: this.Model.Player,
                    setCardMovementModel: setCardMovementViewModel);
            }
        }
    }
}
