namespace Assets.Scripts.Views.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Spans;
    using Assets.Scripts.Views;
    using SimulatorsOfTimeline = Assets.Scripts.Simulators.Timeline;

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
        public override ISpanView Spawn()
        {
            return new MoveCardsToHandFromPileView();
        }

        // - プロパティ

        MoveCardsToHandFromPileModel GetModel(SimulatorsOfTimeline.TimeSpan timeSpan)
        {
            return (MoveCardsToHandFromPileModel)timeSpan.SpanModel;
        }

        // - メソッド

        /// <summary>
        /// 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
        /// 
        /// - 画面上の場札は位置調整される
        /// </summary>
        public override void OnEnter(
            SimulatorsOfTimeline.TimeSpan timeSpan,
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<MovementViewModel> setMovementViewModel)
        {
            var length = gameModelBuffer.IdOfCardsOfPlayersPile[GetModel(timeSpan).Player].Count; // 手札の枚数

            if (GetModel(timeSpan).NumberOfCards <= length)
            {
                // 天辺から取っていく
                var startIndex = length - GetModel(timeSpan).NumberOfCards;
                gameModelBuffer.MoveCardsToHandFromPile(GetModel(timeSpan).Player, startIndex, GetModel(timeSpan).NumberOfCards);

                // もし、場札が空っぽのところへ、手札を配ったのなら、先頭の場札をピックアップする
                if (gameModelBuffer.IndexOfFocusedCardOfPlayers[GetModel(timeSpan).Player] == -1)
                {
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[GetModel(timeSpan).Player] = 0;
                }

                // 場札の位置の再調整（をしないと、手札から移動しない）
                GameModel gameModel = new GameModel(gameModelBuffer);
                gameViewModel.ArrangeHandCards(
                    startSeconds: timeSpan.StartSeconds,
                    duration1: timeSpan.Duration / 2.0f,
                    duration2: timeSpan.Duration / 2.0f,
                    gameModel: gameModel,
                    player: GetModel(timeSpan).Player,
                    setCardMovementModel: setMovementViewModel);
            }
        }
    }
}
