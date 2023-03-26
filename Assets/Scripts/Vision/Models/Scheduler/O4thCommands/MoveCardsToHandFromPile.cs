namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.GameBuffer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
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
        /// <param name="startObj"></param>
        /// <param name="command"></param>
        public MoveCardsToHandFromPile(
            GameSeconds startObj,
            ModelOfThinkingEngineCommand.IModel command)
            : base(startObj, command)
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
            ModelOfGameBuffer.Model gameModelBuffer,
            ModelOfInput.Init inputModel,
            ModelOfScheduler.Model schedulerModel,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimespan)
        {
            var command = (ModelOfThinkingEngineCommand.MoveCardsToHandFromPile)this.CommandOfThinkingEngine;
            var playerObj = command.PlayerObj;

            // 確定：手札の枚数
            var length = gameModelBuffer.Players[command.PlayerObj.AsInt].IdOfCardsOfPlayersPile.Count;

            // 手札がないのに、手札を引こうとしたとき
            if (length < command.NumberOfCards)
            {
                // TODO ★ なぜここにくる？
                // できない指示は無視
                // Debug.Log("[MoveCardsToHandFromPileView OnEnter] できない指示は無視");

                // 制約の解除
                inputModel.Players[playerObj.AsInt].Rights.IsPileCardDrawing = false;
                return;
            }

            // モデル更新：場札への移動
            gameModelBuffer.MoveCardsToHandFromPile(
                playerObj: playerObj,
                startIndexObj: new PlayerPileCardIndex(length - command.NumberOfCards),
                numberOfCards: command.NumberOfCards);
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
                    timeRange: this.TimeRangeObj,
                    playerObj: playerObj,
                    indexOfPickupObj: gameModel.GetIndexOfFocusedCardOfPlayer(playerObj),
                    idOfHandCards: gameModel.GetCardsOfPlayerHand(playerObj),
                    keepPickup: true,
                    setTimespan: setTimespan,
                    onProgressOrNull: (progress) =>
                    {
                        if (1.0f <= progress)
                        {
                            // 制約の解除
                            inputModel.Players[playerObj.AsInt].Rights.IsPileCardDrawing = false;
                        }
                    });
            }
            else
            {
                // 制約の解除
                inputModel.Players[playerObj.AsInt].Rights.IsPileCardDrawing = false;
            }
        }
    }
}
