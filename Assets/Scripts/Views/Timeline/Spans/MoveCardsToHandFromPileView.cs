namespace Assets.Scripts.Views.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Spans;
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
        /// <param name="timeSpan">タイム・スパン</param>
        /// <param name="model">モデル</param>
        internal MoveCardsToHandFromPileView(TimeSpan timeSpan, MoveCardsToHandFromPileModel model)
            : base(timeSpan)
        {
            this.Model = model;
        }

        // - プロパティ

        MoveCardsToHandFromPileModel Model { get; set; }

        // - メソッド

        /// <summary>
        /// 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
        /// 
        /// - 画面上の場札は位置調整される
        /// </summary>
        public override void OnEnter(
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<CardMovementViewModel> setCardMovementModel)
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
                    startSeconds: this.TimeSpan.StartSeconds,
                    duration1: this.TimeSpan.Duration / 2.0f,
                    duration2: this.TimeSpan.Duration / 2.0f,
                    gameModel: gameModel,
                    player: this.Model.Player,
                    setCardMovementModel: setCardMovementModel);
            }
        }
    }
}
