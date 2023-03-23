namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfSchedulerO3rdViewCommand = Assets.Scripts.Vision.Models.Scheduler.O3rdViewCommand;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    class MoveCardsToHandFromPile : ItsAbstract
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="commandOfThinkingEngine"></param>
        public MoveCardsToHandFromPile(ModelOfThinkingEngineCommand.IModel commandOfThinkingEngine)
            : base(commandOfThinkingEngine)
        {
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// 
        /// - 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
        /// - 画面上の場札は位置調整される
        /// </summary>
        public override void GenerateSpan(
            ITask task,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimelineSpan)
        {
            // 確定：手札の枚数
            var length = gameModelBuffer.IdOfCardsOfPlayersPile[GetCommandOfThinkingEngine(task).PlayerObj.AsInt].Count;

            if (length < GetCommandOfThinkingEngine(task).NumberOfCards)
            {
                // できない指示は無視
                // Debug.Log("[MoveCardsToHandFromPileView OnEnter] できない指示は無視");
                return;
            }

            var playerObj = GetCommandOfThinkingEngine(task).PlayerObj;

            // モデル更新：場札への移動
            gameModelBuffer.MoveCardsToHandFromPile(
                playerObj: playerObj,
                startIndexObj: new PlayerPileCardIndex(length - GetCommandOfThinkingEngine(task).NumberOfCards),
                numberOfCards: GetCommandOfThinkingEngine(task).NumberOfCards);
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
                ModelOfSchedulerO3rdViewCommand.ArrangeHandCards.GenerateSpan(
                    startTimeObj: task.TimeRangeObj.StartObj,
                    durationObj: CommandDurationMapping.GetDurationBy(task.CommandOfScheduler.CommandOfThinkingEngine.GetType()),
                    playerObj: playerObj,
                    indexOfPickupObj: gameModel.GetIndexOfFocusedCardOfPlayer(playerObj),
                    idOfHandCards: gameModel.GetCardsOfPlayerHand(playerObj),
                    keepPickup: true,
                    setTimelineSpan: setTimelineSpan);
            }

            // TODO ★ ピックアップしている場札を持ち上げる
            {

            }
        }

        ModelOfThinkingEngineCommand.MoveCardsToHandFromPile GetCommandOfThinkingEngine(ITask task)
        {
            return (ModelOfThinkingEngineCommand.MoveCardsToHandFromPile)task.CommandOfScheduler.CommandOfThinkingEngine;
        }
    }
}
