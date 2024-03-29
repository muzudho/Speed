﻿namespace Assets.Scripts.Scheduler.AnalogCommands.O4thComplex
{
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Models;
    using System.Collections.Generic;
    using UnityEngine;
    using ModelForVisionCommons = Assets.Scripts.Vision.Commons;
    using ModelOfAnalogCommand1stTimelineSpan = Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan;
    using ModelOfAnalogCommands = Assets.Scripts.Scheduler.AnalogCommands;
    using ModelOfAnalogCommands3rdSimplex = Assets.Scripts.Scheduler.AnalogCommands.O3rdSimplex;
    using ModelOfDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;
    using ModelOfGameWriter = Assets.Scripts.ThinkingEngine.Models.Game.Writer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfObservableGame = Assets.Scripts.ThinkingEngine.Models.Game.Observable;

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
        /// <param name="digitalCommand"></param>
        public MoveLikeBoomerang(
            GameSeconds startObj,
            ModelOfDigitalCommands.IModel digitalCommand)
            : base(startObj, digitalCommand)
        {
        }

        // - フィールド

        FocusedHandCard oldHandCardObj;
        int lengthOfHand;

        // 確定：場札から台札へ移動するカード
        IdOfPlayingCards targetToRemoveObj;

        // 台札の新しい天辺の座標
        Vector3 nextTop;

        // - メソッド

        /// <summary>
        /// 準備
        /// </summary>
        public override void Setup(ModelOfObservableGame.Model observableGameModel)
        {
            var digitalCommand = (ModelOfDigitalCommands.MoveCardToCenterStackFromHand)this.DigitalCommand;
            var playerObj = digitalCommand.PlayerObj;

            // 何枚目の場札をピックアップしているか
            this.oldHandCardObj = observableGameModel.GetFocusedHandCardObj(playerObj);

            // カードを抜く前の場札の枚数
            this.lengthOfHand = observableGameModel.GetLengthOfHand(playerObj);

            // 確定：場札から台札へ移動するカード
            this.targetToRemoveObj = observableGameModel.GetCardOfHand(playerObj, this.oldHandCardObj.Index);

            // ピックアップしているカードは、場札から抜くカード
            var placeObj = digitalCommand.PlaceObj;

            //
            // 台札の新しい天辺の座標
            // ======================
            //
            // - (Analog) 相手が台札へ向かって投げた場札が、まだ空中を移動中かも
            //
            this.nextTop = ModelForVisionCommons.CreatePositionOfNewCenterStackCard(
                                placeObj: placeObj,
                                observableGameModel: observableGameModel) + ModelForVisionCommons.yOfCardThickness.ToMutable();
        }

        /// <summary>
        /// タイムスパン作成・登録
        /// </summary>
        public override List<ModelOfAnalogCommand1stTimelineSpan.IModel> CreateTimespanList(
            ModelOfGameWriter.Model gameModelWriter,
            ModelOfInput.Init inputModel,
            ModelOfAnalogCommands.Model schedulerModel)
        {
            var result = new List<ModelOfAnalogCommand1stTimelineSpan.IModel>();

            var digitalCommand = (ModelOfDigitalCommands.MoveCardToCenterStackFromHand)this.DigitalCommand;

            var playerObj = digitalCommand.PlayerObj;

            // 範囲外は無視
            if (this.oldHandCardObj.Index < HandCardIndex.First || this.lengthOfHand <= this.oldHandCardObj.Index.AsInt)
            {
                return result;
            }

            // ピックアップしているカードは、場札から抜くカード
            var placeObj = digitalCommand.PlaceObj;

            // 確定：（抜いた後に）次にピックアップするカード（が先頭から何枚目か）
            FocusedHandCard nextFocusedHandCardObj;
            {
                // 確定：抜いた後の場札の数
                int lengthAfterRemove;
                {
                    lengthAfterRemove = this.lengthOfHand - 1;
                }

                if (lengthAfterRemove <= this.oldHandCardObj.Index.AsInt) // 範囲外アクセス防止対応
                {
                    // 一旦、最後尾へ
                    nextFocusedHandCardObj = new FocusedHandCard(true, new HandCardIndex(lengthAfterRemove - 1));
                }
                else
                {
                    // そのまま
                    nextFocusedHandCardObj = new FocusedHandCard(true, this.oldHandCardObj.Index);
                }
            }

            // モデル更新：場札を１枚抜く
            gameModelWriter.GetPlayer(playerObj).RemoveCardAtOfHand(this.oldHandCardObj.Index);

            // 確定：場札の枚数
            var lengthOfHandCards = gameModelWriter.GetPlayer(playerObj).GetLengthOfHandCards();

            // 確定：抜いたあとの場札リスト
            var idOfHandCardsAfterRemove = gameModelWriter.GetPlayer(playerObj).GetCardsOfHand();

            // モデル更新：何枚目の場札をピックアップしているか
            gameModelWriter.GetPlayer(playerObj).UpdateFocusedHandCardObj(nextFocusedHandCardObj);

            // 確定：前の台札の天辺のカード
            IdOfPlayingCards idOfPreviousTop = gameModelWriter.GetCenterStack(placeObj).GetTopCard();

            // モデル更新：次に、台札として置く
            var indexOfCenterStack = gameModelWriter.GetCenterStack(placeObj).GetLength();
            gameModelWriter.GetCenterStack(placeObj).AddCard(this.targetToRemoveObj);

            // 台札へ置く
            result.Add(ModelOfAnalogCommands3rdSimplex.PutCardToCenterStack.CreateTimespan(
                timeRange: new ModelOfAnalogCommand1stTimelineSpan.Range(
                    start: this.TimeRangeObj.StartObj,
                    duration: new GameSeconds(DurationMapping.GetDurationBy(this.DigitalCommand.GetType()).AsFloat / 2.0f)),
                playerObj: playerObj,
                target: this.targetToRemoveObj,
                nextTop: this.nextTop,
                onProgressOrNull: (progress) =>
                {
                    // 下のカードの数が、自分のカードの数の隣でなければ
                    // Debug.Log($"テストA indexOfCenterStack:{indexOfCenterStack}");
                    if (0 < indexOfCenterStack)
                    {
                        // Debug.Log($"テストB placeObj:{placeObj.AsInt}");

                        // 下のカード
                        var previousCard = gameModelWriter.GetCenterStack(placeObj).GetCard(indexOfCenterStack);
                        // Debug.Log($"テストC topCard:{previousCard.Number()} pickupCard:{this.targetToRemoveObj.Number()}");

                        // 隣ではないか？
                        if (!CardNumberHelper.IsNextNumber(
                            topCard: previousCard,
                            pickupCard: this.targetToRemoveObj))
                        {
                            Debug.Log($"置いたカードが隣ではなかった topCard:{previousCard.Number()} pickupCard:{this.targetToRemoveObj.Number()}");

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
            result.AddRange(ModelOfAnalogCommands3rdSimplex.ArrangeHandCards.CreateTimespanList(
                timeRange: new ModelOfAnalogCommand1stTimelineSpan.Range(
                    start: new GameSeconds(this.TimeRangeObj.StartObj.AsFloat + DurationMapping.GetDurationBy(this.DigitalCommand.GetType()).AsFloat / 2.0f),
                    duration: new GameSeconds(DurationMapping.GetDurationBy(this.DigitalCommand.GetType()).AsFloat / 2.0f)),
                playerObj: playerObj,
                indexOfPickupObj: nextFocusedHandCardObj.Index, // 抜いたカードではなく、次にピックアップするカードを指定。 × indexToRemove
                idOfHandCards: idOfHandCardsAfterRemove,
                keepPickup: true,
                onProgressOrNull: null));

            return result;
        }
    }
}
