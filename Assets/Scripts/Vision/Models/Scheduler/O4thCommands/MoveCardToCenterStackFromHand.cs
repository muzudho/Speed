namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfSchedulerO3rdViewCommand = Assets.Scripts.Vision.Models.Scheduler.O3rdViewCommand;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    /// <summary>
    /// ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
    /// </summary>
    class MoveCardToCenterStackFromHand : ItsAbstract
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="commandOfThinkingEngine"></param>
        public MoveCardToCenterStackFromHand(
            GameSeconds startObj,
            ModelOfThinkingEngineCommand.IModel commandOfThinkingEngine)
            : base(startObj, commandOfThinkingEngine)
        {
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// 
        /// - ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
        /// </summary>
        /// <param name="player">何番目のプレイヤー</param>
        /// <param name="place">右なら0、左なら1</param>
        public override void GenerateSpan(
            GameModelBuffer gameModelBuffer,
            ModelOfInput.Init inputModel,
            ModelOfScheduler.Model schedulerModel,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimelineSpan)
        {
            var command = (ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand)this.CommandOfThinkingEngine;

            var gameModel = new ModelOfGame.Default(gameModelBuffer);
            var playerObj = command.PlayerObj;

            // ピックアップしているカードは、場札から抜くカード
            GetIndexOfFocusedHandCard(
                gameModelBuffer: gameModelBuffer,
                playerObj: playerObj,
                (indexToRemoveObj) =>  // 確定：場札から抜くのは何枚目
                {
                    var placeObj = command.PlaceObj;

                    // 確定：（抜いた後に）次にピックアップするカード（が先頭から何枚目か）
                    HandCardIndex indexOfNextPickObj;
                    {
                        // 確定：抜いた後の場札の数
                        int lengthAfterRemove;
                        {
                            // 抜く前の場札の数
                            var lengthBeforeRemove = gameModelBuffer.IdOfCardsOfPlayersHand[playerObj.AsInt].Count;
                            lengthAfterRemove = lengthBeforeRemove - 1;
                        }

                        if (lengthAfterRemove <= indexToRemoveObj.AsInt) // 範囲外アクセス防止対応
                        {
                            // 一旦、最後尾へ
                            indexOfNextPickObj = new HandCardIndex(lengthAfterRemove - 1);
                        }
                        else
                        {
                            // そのまま
                            indexOfNextPickObj = indexToRemoveObj;
                        }
                    }

                    // 確定：場札から台札へ移動するカード
                    var targetToRemoveObj = gameModelBuffer.IdOfCardsOfPlayersHand[playerObj.AsInt][indexToRemoveObj.AsInt];

                    // モデル更新：場札を１枚抜く
                    gameModelBuffer.RemoveCardAtOfPlayerHand(playerObj, indexToRemoveObj);

                    // 確定：場札の枚数
                    var lengthOfHandCards = gameModel.GetLengthOfPlayerHandCards(playerObj);

                    // 確定：抜いたあとの場札リスト
                    var idOfHandCardsAfterRemove = gameModel.GetCardsOfPlayerHand(playerObj);

                    // モデル更新：何枚目の場札をピックアップしているか
                    gameModelBuffer.IndexOfFocusedCardOfPlayersObj[playerObj.AsInt] = indexOfNextPickObj;

                    // 確定：前の台札の天辺のカード
                    IdOfPlayingCards idOfPreviousTop = gameModel.GetTopOfCenterStack(placeObj);

                    // モデル更新：次に、台札として置く
                    gameModelBuffer.AddCardOfCenterStack(placeObj, targetToRemoveObj);

                    // 台札へ置く
                    setTimelineSpan(ModelOfSchedulerO3rdViewCommand.PutCardToCenterStack.GenerateSpan(
                        timeRange: new ModelOfSchedulerO1stTimelineSpan.Range(
                            start: this.TimeRangeObj.StartObj,
                            duration: new GameSeconds(CommandDurationMapping.GetDurationBy(this.CommandOfThinkingEngine.GetType()).AsFloat / 2.0f)),
                        playerObj: playerObj,
                        placeObj: placeObj,
                        target: targetToRemoveObj,
                        idOfPreviousTop: idOfPreviousTop,
                        onFinishedOrNull: ()=>
                        {
                            // TODO ★ 終了時の処理
                            inputModel.Players[playerObj.AsInt].Rights.IsThrowingCardIntoCenterStack = false;                            
                        }));

                    // 場札の位置調整（をしないと歯抜けになる）
                    ModelOfSchedulerO3rdViewCommand.ArrangeHandCards.GenerateSpan(
                        timeRange: new ModelOfSchedulerO1stTimelineSpan.Range(
                            start: new GameSeconds(this.TimeRangeObj.StartObj.AsFloat + CommandDurationMapping.GetDurationBy(this.CommandOfThinkingEngine.GetType()).AsFloat / 2.0f),
                            duration: new GameSeconds(CommandDurationMapping.GetDurationBy(this.CommandOfThinkingEngine.GetType()).AsFloat / 2.0f)),
                        playerObj: playerObj,
                        indexOfPickupObj: indexOfNextPickObj, // 抜いたカードではなく、次にピックアップするカードを指定。 × indexToRemove
                        idOfHandCards: idOfHandCardsAfterRemove,
                        keepPickup: true,
                        setTimelineSpan: setTimelineSpan); // 場札
                });
        }

        private void GetIndexOfFocusedHandCard(GameModelBuffer gameModelBuffer, Player playerObj, LazyArgs.SetValue<HandCardIndex> setIndex)
        {
            var handIndex = gameModelBuffer.IndexOfFocusedCardOfPlayersObj[playerObj.AsInt]; // 何枚目の場札をピックアップしているか
            if (handIndex < Commons.HandCardIndexFirst || gameModelBuffer.IdOfCardsOfPlayersHand[playerObj.AsInt].Count <= handIndex.AsInt) // 範囲外は無視
            {
                return;
            }

            setIndex(handIndex);
        }
    }
}
