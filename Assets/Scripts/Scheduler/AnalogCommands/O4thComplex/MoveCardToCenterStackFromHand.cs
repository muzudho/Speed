namespace Assets.Scripts.Scheduler.AnalogCommands.O4thComplex
{
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Models;
    using System.Collections.Generic;
    using UnityEngine;
    using ModelOfAnalogCommand1stTimelineSpan = Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan;
    using ModelOfAnalogCommand3rdSimplex = Assets.Scripts.Scheduler.AnalogCommands.O3rdSimplex;
    using ModelOfAnalogCommands = Assets.Scripts.Scheduler.AnalogCommands;
    using ModelOfDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;
    using ModelOfGameWriter = Assets.Scripts.ThinkingEngine.Models.Game.Writer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelForVisionCommons = Assets.Scripts.Vision.Commons;

    /// <summary>
    /// ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する（確定）
    /// 
    /// - 置くのをキャンセルする（ブーメランする）といった動きは行わない
    /// - これから置く場札の数は、これから置く先の台札の数から連続だ（確定）
    /// </summary>
    class MoveCardToCenterStackFromHand : ItsAbstract
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startObj"></param>
        /// <param name="digitalCommand"></param>
        public MoveCardToCenterStackFromHand(
            GameSeconds startObj,
            ModelOfDigitalCommands.IModel digitalCommand)
            : base(startObj, digitalCommand)
        {
        }

        // - フィールド

        FocusedHandCard oldHandCardObj;

        // 抜く前の場札の数
        int lengthOfHand;

        // 確定：場札から台札へ移動するカード
        IdOfPlayingCards targetToRemoveObj;

        // 台札の新しい天辺の座標
        Vector3 nextTop;

        // - メソッド

        /// <summary>
        /// 準備
        /// </summary>
        public override void Setup(ModelOfGameBuffer.Model gameModelBuffer)
        {
            var digitalCommand = (ModelOfDigitalCommands.MoveCardToCenterStackFromHand)this.DigitalCommand;
            var playerObj = digitalCommand.PlayerObj;

            // 何枚目の場札をピックアップしているか
            this.oldHandCardObj = gameModelBuffer.GetPlayer(playerObj).FocusedHandCardObj;

            // 抜く前の場札の数
            this.lengthOfHand = gameModelBuffer.GetPlayer(playerObj).IdOfCardsOfHand.Count;

            // 確定：場札から台札へ移動するカード
            this.targetToRemoveObj = gameModelBuffer.GetPlayer(playerObj).IdOfCardsOfHand[this.oldHandCardObj.Index.AsInt];

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
                                gameModelBuffer: gameModelBuffer) + ModelForVisionCommons.yOfCardThickness.ToMutable();
        }

        /// <summary>
        /// タイムスパン作成・登録
        /// </summary>
        public override List<ModelOfAnalogCommand1stTimelineSpan.IModel> CreateTimespanList(
            ModelOfGameBuffer.Model _gameModelBuffer,
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

            //
            // 場札１枚減らす
            // ==============
            //
            gameModelWriter.GetPlayer(playerObj).RemoveCardAtOfHand(this.oldHandCardObj.Index);

            // 確定：場札の枚数
            var lengthOfHandCards = gameModelWriter.GetPlayer(playerObj).GetLengthOfHandCards();

            // 確定：抜いたあとの場札リスト
            var idOfHandCardsAfterRemove = gameModelWriter.GetPlayer(playerObj).GetCardsOfHand();

            // モデル更新：何枚目の場札をピックアップしているか
            // ================================================
            gameModelWriter.GetPlayer(playerObj).UpdateFocusedHandCardObj(nextFocusedHandCardObj);

            // 確定：前の台札の天辺のカード
            IdOfPlayingCards idOfPreviousTop = gameModelWriter.GetCenterStack(placeObj).GetTopCard();

            //
            // 台札１枚増やす
            // ==============
            //
            // これから置く札の台札でのインデックス
            var indexOnCenterStackToNextCard = gameModelWriter.GetCenterStack(placeObj).GetLength();
            gameModelWriter.GetCenterStack(placeObj).AddCard(this.targetToRemoveObj);

            // 台札へ置く
            result.Add(ModelOfAnalogCommand3rdSimplex.PutCardToCenterStack.CreateTimespan(
                timeRange: new ModelOfAnalogCommand1stTimelineSpan.Range(
                    start: this.TimeRangeObj.StartObj,
                    duration: new GameSeconds(DurationMapping.GetDurationBy(this.DigitalCommand.GetType()).AsFloat / 2.0f)),
                playerObj: playerObj,
                target: this.targetToRemoveObj,
                nextTop: this.nextTop,
                onProgressOrNull: (progress) =>
                {
                    if (1.0f <= progress)
                    {
                        // 制約の解除
                        inputModel.Players[playerObj.AsInt].Rights.IsThrowingCardIntoCenterStack = false;
                    }
                }));

            // 場札の位置調整（をしないと歯抜けになる）
            result.AddRange(ModelOfAnalogCommand3rdSimplex.ArrangeHandCards.CreateTimespanList(
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

