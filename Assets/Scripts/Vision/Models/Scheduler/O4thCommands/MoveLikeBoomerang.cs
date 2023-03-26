namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using UnityEngine;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.GameBuffer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfSchedulerO3rdViewCommand = Assets.Scripts.Vision.Models.Scheduler.O3rdViewCommand;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    /// <summary>
    /// 場札から、台札へ向かったカードが、場札へまた戻ってくる動き
    /// </summary>
    internal class MoveLikeBoomerang : ItsAbstract
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startObj"></param>
        /// <param name="command"></param>
        public MoveLikeBoomerang(
            GameSeconds startObj,
            ModelOfThinkingEngineCommand.IModel command)
            : base(startObj, command)
        {
        }

        // - メソッド

        /// <summary>
        /// タイムスパン作成・登録
        /// </summary>
        public override void GenerateSpan(
            ModelOfGameBuffer.Model gameModelBuffer,
            ModelOfInput.Init inputModel,
            ModelOfScheduler.Model schedulerModel,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimespan)
        {
            var command = (ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand)this.CommandOfThinkingEngine;

            var gameModel = new ModelOfGame.Default(gameModelBuffer);
            var playerObj = command.PlayerObj;
            var indexToRemoveObj = gameModelBuffer.GetPlayer(playerObj).IndexOfFocusedCard; // 何枚目の場札をピックアップしているか

            // 範囲外は無視
            if (indexToRemoveObj < Commons.HandCardIndexFirst || gameModelBuffer.GetPlayer(playerObj).IdOfCardsOfHand.Count <= indexToRemoveObj.AsInt)
            {
                return;
            }

            // ピックアップしているカードは、場札から抜くカード
            var placeObj = command.PlaceObj;

            // 確定：（抜いた後に）次にピックアップするカード（が先頭から何枚目か）
            HandCardIndex indexOfNextPickObj;
            {
                // 確定：抜いた後の場札の数
                int lengthAfterRemove;
                {
                    // 抜く前の場札の数
                    var lengthBeforeRemove = gameModelBuffer.GetPlayer(playerObj).IdOfCardsOfHand.Count;
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
            var targetToRemoveObj = gameModelBuffer.GetPlayer(playerObj).IdOfCardsOfHand[indexToRemoveObj.AsInt];

            // モデル更新：場札を１枚抜く
            gameModelBuffer.GetPlayer(playerObj).RemoveCardAtOfHand(indexToRemoveObj);

            // 確定：場札の枚数
            var lengthOfHandCards = gameModel.GetPlayer(playerObj).GetLengthOfHandCards();

            // 確定：抜いたあとの場札リスト
            var idOfHandCardsAfterRemove = gameModel.GetPlayer(playerObj).GetCardsOfHand();

            // モデル更新：何枚目の場札をピックアップしているか
            gameModelBuffer.GetPlayer(playerObj).IndexOfFocusedCard = indexOfNextPickObj;

            // 確定：前の台札の天辺のカード
            IdOfPlayingCards idOfPreviousTop = gameModel.GetCenterStack(placeObj).GetTopCard();

            // モデル更新：次に、台札として置く
            var indexOfCenterStack = gameModelBuffer.GetCenterStack(placeObj).GetLength();
            gameModelBuffer.GetCenterStack(placeObj).AddCard(targetToRemoveObj);

            // 台札へ置く
            setTimespan(ModelOfSchedulerO3rdViewCommand.PutCardToCenterStack.GenerateSpan(
                timeRange: new ModelOfSchedulerO1stTimelineSpan.Range(
                    start: this.TimeRangeObj.StartObj,
                    duration: new GameSeconds(CommandDurationMapping.GetDurationBy(this.CommandOfThinkingEngine.GetType()).AsFloat / 2.0f)),
                playerObj: playerObj,
                placeObj: placeObj,
                target: targetToRemoveObj,
                idOfPreviousTop: idOfPreviousTop,
                onProgressOrNull: (progress) =>
                {
                    // 下のカードの数が、自分のカードの数の隣でなければ
                    // Debug.Log($"テストA indexOfCenterStack:{indexOfCenterStack}");
                    if (0 < indexOfCenterStack)
                    {
                        // Debug.Log($"テストB placeObj:{placeObj.AsInt}");

                        // 下のカード
                        var previousCard = gameModelBuffer.GetCenterStack(placeObj).GetCard(indexOfCenterStack);
                        // Debug.Log($"テストC topCard:{previousCard.Number()} pickupCard:{targetToRemoveObj.Number()}");

                        // 隣ではないか？
                        if (!CardNumberHelper.IsNext(
                            topCard: previousCard,
                            pickupCard: targetToRemoveObj))
                        {
                            Debug.Log($"置いたカードが隣ではなかった topCard:{previousCard.Number()} pickupCard:{targetToRemoveObj.Number()}");

                            // TODO ★ この動作をキャンセルし、元に戻す動作に変えたい

                            // 即実行
                            // ======

                            //// コマンド作成（思考エンジン用）
                            //var commandOfThinkingEngine = new ModelOfThinkingEngineCommand.Ignore();

                            //// コマンド作成（画面用）
                            //var commandOfScheduler = new MoveBackCardToHand(
                            //    startObj: GameSeconds.Zero,
                            //    command: commandOfThinkingEngine);

                            //// タイムスパン作成・登録
                            //commandOfScheduler.GenerateSpan(
                            //    gameModelBuffer: gameModelBuffer,
                            //    inputModel: inputModel,
                            //    schedulerModel: schedulerModel,
                            //    setTimespan: setTimespan);
                        }
                    }

                    if (1.0f <= progress)
                    {
                        // 制約の解除
                        inputModel.Players[playerObj.AsInt].Rights.IsThrowingCardIntoCenterStack = false;
                    }
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
                setTimespan: setTimespan,
                onProgressOrNull: null);
        }
    }
}
