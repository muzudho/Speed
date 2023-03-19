namespace Assets.Scripts.Vision.Models.Timeline.O4thGameOperation
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfTimelineO1stSpan = Assets.Scripts.Vision.Models.Timeline.O1stSpan;
    using ModelOfTimelineO3rdSpanGenerator = Assets.Scripts.Vision.Models.Timeline.O3rdSpanGenerator;

    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    class MoveCardsToHandFromPileView : ItsAbstract
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override IModel NewThis()
        {
            return new MoveCardsToHandFromPileView();
        }

        // - プロパティ

        MoveCardsToHandFromPileModel GetModel(IGameOperationSpan timedGenerator)
        {
            return (MoveCardsToHandFromPileModel)timedGenerator.TimedCommandArg.CommandArg;
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// 
        /// - 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
        /// - 画面上の場札は位置調整される
        /// </summary>
        public override void CreateSpan(
            IGameOperationSpan timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfTimelineO1stSpan.IBasecaseSpan> setViewMovement)
        {
            // 確定：手札の枚数
            var length = gameModelBuffer.IdOfCardsOfPlayersPile[GetModel(timedGenerator).PlayerObj.AsInt].Count;

            if (length < GetModel(timedGenerator).NumberOfCards)
            {
                // できない指示は無視
                // Debug.Log("[MoveCardsToHandFromPileView OnEnter] できない指示は無視");
                return;
            }

            var playerObj = GetModel(timedGenerator).PlayerObj;

            // モデル更新：場札への移動
            gameModelBuffer.MoveCardsToHandFromPile(
                playerObj: playerObj,
                startIndexObj: new PlayerPileCardIndex(length - GetModel(timedGenerator).NumberOfCards),
                numberOfCards: GetModel(timedGenerator).NumberOfCards);
            // 場札は１枚以上になる

            // モデル更新：もし、ピックアップ場札がなかったら、先頭の場札をピックアップする
            //
            // - 初回配布のケース
            // - 場札無しの勝利後に配ったケース
            if (gameModelBuffer.IndexOfFocusedCardOfPlayersObj[playerObj.AsInt] == Commons.HandCardIndexNoSelected)
            {
                gameModelBuffer.IndexOfFocusedCardOfPlayersObj[playerObj.AsInt] = Commons.HandCardIndexFirst;
            }

            ModelOfGame.Default gameModel = new ModelOfGame.Default(gameModelBuffer);

            // 確定：場札の枚数
            int numberOfCards = gameModel.GetLengthOfPlayerHandCards(playerObj);

            // ビュー：場札の位置の再調整（をしないと、手札から移動しない）
            if (0 < numberOfCards)
            {
                ModelOfTimelineO3rdSpanGenerator.ArrangeHandCards.GenerateSpan(
                    startSeconds: timedGenerator.StartSeconds,
                    duration: timedGenerator.TimedCommandArg.Duration,
                    playerObj: playerObj,
                    indexOfPickupObj: gameModel.GetIndexOfFocusedCardOfPlayer(playerObj),
                    idOfHandCards: gameModel.GetCardsOfPlayerHand(playerObj),
                    keepPickup: true,
                    setSpanToLerp: setViewMovement);
            }

            // TODO ★ ピックアップしている場札を持ち上げる
            {

            }
        }
    }
}
