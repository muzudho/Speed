namespace Assets.Scripts.Vision.Models.Timeline.SpanOfLerp.GeneratorGenerator
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using Assets.Scripts.Vision.Models.Timeline.SpanOfLerp.Generator;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using SimulatorsOfTimeline = Assets.Scripts.Vision.Models.Timeline.SpanOfLerp.TimedGenerator;
    using VisionOfTimelineO4thElement = Assets.Scripts.Vision.Models.Timeline.O4thElement;

    /// <summary>
    /// ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
    /// </summary>
    class MoveCardToCenterStackFromHandView : AbstractSpanGenerator
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override ISpanGenerator Spawn()
        {
            return new MoveCardToCenterStackFromHandView();
        }

        // - プロパティ

        MoveCardToCenterStackFromHandModel GetModel(SimulatorsOfTimeline.TimedGenerator timedGenerator)
        {
            return (MoveCardToCenterStackFromHandModel)timedGenerator.TimedCommandArg.CommandArg;
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// 
        /// - ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
        /// </summary>
        /// <param name="player">何番目のプレイヤー</param>
        /// <param name="place">右なら0、左なら1</param>
        public override void CreateSpanToLerp(
            SimulatorsOfTimeline.TimedGenerator timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<VisionOfTimelineO4thElement.Model> setViewMovement)
        {
            var gameModel = new ModelOfGame.Default(gameModelBuffer);
            var playerObj = GetModel(timedGenerator).PlayerObj;

            // ピックアップしているカードは、場札から抜くカード
            GetIndexOfFocusedHandCard(
                gameModelBuffer: gameModelBuffer,
                playerObj: playerObj,
                (indexToRemoveObj) =>  // 確定：場札から抜くのは何枚目
                {
                    var placeObj = GetModel(timedGenerator).PlaceObj;

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
                    setViewMovement(PutCardToCenterStack.Generate(
                        startSeconds: timedGenerator.StartSeconds,
                        duration: timedGenerator.TimedCommandArg.Duration / 2.0f,
                        playerObj: playerObj,
                        placeObj: placeObj,
                        target: targetToRemoveObj,
                        idOfPreviousTop));

                    // 場札の位置調整（をしないと歯抜けになる）
                    ArrangeHandCards.Generate(
                        startSeconds: timedGenerator.StartSeconds + timedGenerator.TimedCommandArg.Duration / 2.0f,
                        duration: timedGenerator.TimedCommandArg.Duration / 2.0f,
                        playerObj: playerObj,
                        indexOfPickupObj: indexOfNextPickObj, // 抜いたカードではなく、次にピックアップするカードを指定。 × indexToRemove
                        idOfHandCards: idOfHandCardsAfterRemove,
                        keepPickup: true,
                        setSpanToLerp: setViewMovement); // 場札

                    // TODO ★ ピックアップしている場札を持ち上げる
                    {

                    }

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

        private bool CanRemoveHandCardAt(
            GameModelBuffer gameModelBuffer,
            int player,
            int indexToRemove)
        {
            // 抜く前の場札の数
            var lengthBeforeRemove = gameModelBuffer.IdOfCardsOfPlayersHand[player].Count;
            if (indexToRemove < 0 || lengthBeforeRemove <= indexToRemove)
            {
                // 抜くのに失敗
                return false;
            }


            return true;
        }
    }
}
