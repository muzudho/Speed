namespace Assets.Scripts.Gui.SpanOfLerp.GeneratorGenerator
{
    using Assets.Scripts.Gui.SpanOfLerp.Generator;
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.CommandArgs;
    using Assets.Scripts.Views.Timeline;
    using UnityEngine;
    using SimulatorsOfTimeline = Assets.Scripts.Gui.SpanOfLerp.TimedGenerator;

    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    class MoveCardsToHandFromPileView : AbstractSpanGenerator
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override ISpanGenerator Spawn()
        {
            return new MoveCardsToHandFromPileView();
        }

        // - プロパティ

        MoveCardsToHandFromPileModel GetModel(SimulatorsOfTimeline.TimedGenerator timedGenerator)
        {
            return (MoveCardsToHandFromPileModel)timedGenerator.CommandArgs;
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// 
        /// - 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
        /// - 画面上の場札は位置調整される
        /// </summary>
        public override void CreateSpanToLerp(
            SimulatorsOfTimeline.TimedGenerator timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<SpanToLerp> setViewMovement)
        {
            // 確定：手札の枚数
            var length = gameModelBuffer.IdOfCardsOfPlayersPile[GetModel(timedGenerator).Player].Count;

            if (length < GetModel(timedGenerator).NumberOfCards)
            {
                // できない指示は無視
                Debug.Log("[MoveCardsToHandFromPileView OnEnter] できない指示は無視");
                return;
            }

            var player = GetModel(timedGenerator).Player;

            // モデル更新：場札への移動
            gameModelBuffer.MoveCardsToHandFromPile(
                player: player,
                startIndex: length - GetModel(timedGenerator).NumberOfCards,
                numberOfCards: GetModel(timedGenerator).NumberOfCards);
            // 場札は１枚以上になる

            // モデル更新：もし、ピックアップ場札がなかったら、先頭の場札をピックアップする
            //
            // - 初回配布のケース
            // - 場札無しの勝利後に配ったケース
            if (gameModelBuffer.IndexOfFocusedCardOfPlayers[player] == -1)
            {
                gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = 0;
            }

            GameModel gameModel = new GameModel(gameModelBuffer);

            // 確定：場札の枚数
            int numberOfCards = gameModel.GetLengthOfPlayerHandCards(player);

            // ビュー：場札の位置の再調整（をしないと、手札から移動しない）
            if (0 < numberOfCards)
            {
                ArrangeHandCards.Generate(
                    startSeconds: timedGenerator.StartSeconds,
                    duration: timedGenerator.Duration,
                    player: player,
                    indexOfPickup: gameModel.GetIndexOfFocusedCardOfPlayer(player),
                    idOfHandCards: gameModel.GetCardsOfPlayerHand(player),
                    keepPickup: true,
                    setSpanToLerp: setViewMovement);
            }

            // TODO ★ ピックアップしている場札を持ち上げる
            {

            }
        }
    }
}
