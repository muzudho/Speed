﻿namespace Assets.Scripts.Vision.Models.Input
{
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Behaviours;
    using System;
    using UnityEngine;
    using ModelOfDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;
    using ModelOfObservableGame = Assets.Scripts.ThinkingEngine.Models.Game.Observable.Model;
    using ModelOfScheduler = Assets.Scripts.Scheduler.AnalogCommands;
    using ModelOfThinkingEngine = Assets.Scripts.ThinkingEngine.Models;
    using ScriptOfThinkingEngine = Assets.Scripts.ThinkingEngine;

    /// <summary>
    /// プレイヤーの入力
    /// 
    /// - 編集可
    /// </summary>
    internal class Player
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="meaning"></param>
        internal Player(
            ModelOfThinkingEngine.Player playerIdObj,
            ModelOfThinkingEngine.CenterStackPlace nearCenterStackPlace,
            ModelOfThinkingEngine.CenterStackPlace farCenterStackPlace,
            Meaning meaning)
        {
            this.PlayerIdObj = playerIdObj;
            this.NearCenterStackPlace = nearCenterStackPlace;
            this.FarCenterStackPlace = farCenterStackPlace;
            this.Meaning = meaning;

            this.CleanUp();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        internal void CleanUp()
        {
            this.Rights.CleanUp();
        }

        // - プロパティ

        /// <summary>
        /// Id
        /// </summary>
        internal ModelOfThinkingEngine.Player PlayerIdObj { get; private set; }

        /// <summary>
        /// 入力の権利
        /// </summary>
        internal Rights Rights { get; private set; } = new Rights();

        /// <summary>
        /// コンピューター・プレイヤーか？
        /// 
        /// - 編集可
        /// - コンピューターなら Computer インスタンス
        /// - コンピューターでなければヌル
        /// </summary>
        internal ScriptOfThinkingEngine.Computer Computer { get; set; } = null;    // new Computer(0), new Computer(1)

        /// <summary>
        /// 入力の意味
        /// 
        /// - プレイヤー別
        /// </summary>
        internal Meaning Meaning { get; private set; }

        #region プロパティ（台札の位置）
        /// <summary>
        /// 自分に近い方の台札
        /// </summary>
        internal ModelOfThinkingEngine.CenterStackPlace NearCenterStackPlace { get; private set; }

        /// <summary>
        /// 自分から遠い方の台札
        /// </summary>
        internal ModelOfThinkingEngine.CenterStackPlace FarCenterStackPlace { get; private set; }

        ModelOfThinkingEngine.CenterStackPlace GetCenterStackPlace(NearFar nearFar)
        {
            switch (nearFar)
            {
                case NearFar.Near:
                    return this.NearCenterStackPlace;

                case NearFar.Far:
                    return this.FarCenterStackPlace;

                default:
                    throw new InvalidOperationException($"undexpected near_far:{nearFar}");
            }
        }
        #endregion

        // - メソッド

        /// <summary>
        /// 入力を翻訳
        /// </summary>
        internal void Translate(ModelOfObservableGame gameModel)
        {
            // キー入力の解析：クリアー
            this.Meaning.Clear();

            // - 対局停止中か？
            this.Rights.IsGameInactive = !gameModel.IsGameActive;

            if (!this.Rights.IsHandled())
            {
                if (this.Computer == null)
                {
                    // キー入力の解析：人間の入力を受付
                    this.Meaning.UpdateFromInput();
                }
                else
                {
                    // コンピューター・プレイヤーが思考して、操作を決める
                    this.Computer.Think(gameModel);

                    // キー入力の解析：コンピューターからの入力を受付
                    this.Meaning.Overwrite(
                        moveCardToCenterStackNearMe: this.Computer.MoveCardToCenterStackNearMe,
                        moveCardToFarCenterStack: this.Computer.MoveCardToFarCenterStack,
                        pickupCardToForward: this.Computer.PickupCardToForward,
                        pickupCardToBackward: this.Computer.PickupCardToBackward,
                        drawing: this.Computer.Drawing);
                }
            }

            // スパン時間消化
            if (0.0f < this.Rights.TimeOfRestObj.AsFloat)
            {
                // 負数になっても気にしない
                this.Rights.TimeOfRestObj = new GameSeconds(this.Rights.TimeOfRestObj.AsFloat - Time.deltaTime);
            }
        }

        /// <summary>
        /// 台札へ置く
        /// </summary>
        /// <param name="nearOrFarOfCenterStack">自分に近い方の台札、または、自分から遠い方の台札</param>
        internal void MoveCardToCenterStackFromHand(
            NearFar nearOrFarOfCenterStack,
            ModelOfObservableGame gameModel,
            StalemateManager stalemateManager,
            ModelOfScheduler.Model schedulerModel)
        {
            if (!this.Rights.IsHandled() &&
                !stalemateManager.IsStalemate &&
                this.Meaning.MoveCardToCenterStack(nearOrFarOfCenterStack) &&
                LegalMove.CanPutCardToCenterStack(gameModel, this.PlayerIdObj, gameModel.GetPlayer(this.PlayerIdObj).GetFocusedHandCardObj().Index, this.GetCenterStackPlace(nearOrFarOfCenterStack)))
            {
                var playerObj = this.PlayerIdObj;
                var placeObj = this.GetCenterStackPlace(nearOrFarOfCenterStack);

                // TODO ブーメラン判定すると、カードを置かなくなる？
                //if (CardMoveHelper.IsBoomerang(gameModel, playerObj, placeObj, out IdOfPlayingCards previousCard))
                //{
                //    // ブーメラン
                //}
                //else
                //{
                    // ピックアップ中の場札を抜いて、台札へ積み上げる
                    var digitalCommand = new ModelOfDigitalCommands.MoveCardToCenterStackFromHand(
                        playerObj: playerObj,
                        placeObj: placeObj);

                    // 制約の付加
                    this.Rights.IsThrowingCardIntoCenterStack = true;

                    schedulerModel.Timeline.AddCommand(
                        startObj: gameModel.ElapsedSeconds,
                        digitalCommand: digitalCommand);
                //}
            }
        }

        /// <summary>
        /// 隣のカードをピックアップするように変えます
        /// </summary>
        internal void PickupCardToNext(
            PickingDirection pickingDirection,
            ModelOfObservableGame gameModel,
            StalemateManager stalemateManager,
            ModelOfScheduler.Model schedulerModel)
        {
            // 制約：
            //      場札が２枚以上あるときに限る
            if (2 <= gameModel.GetPlayer(this.PlayerIdObj).GetCardsOfHand().Count)
            {
                var command = new ModelOfDigitalCommands.MoveFocusToNextCard(
                    playerObj: this.PlayerIdObj,
                    directionObj: pickingDirection);

                // 制約の付加
                this.Rights.IsPickupCartToNext = true;

                schedulerModel.Timeline.AddCommand(
                    startObj: gameModel.ElapsedSeconds,
                    digitalCommand: command);
            }
        }

        /// <summary>
        /// 手札を引く
        /// </summary>
        internal void DrawingHandCardFromPileCard(
            ModelOfObservableGame gameModel,
            ModelOfScheduler.Model schedulerModel)
        {
            // 場札を並べる
            var command = new ModelOfDigitalCommands.MoveCardsToHandFromPile(
                playerObj: this.PlayerIdObj,
                numberOfCards: 1);

            // 制約の付加
            this.Rights.IsPileCardDrawing = true;

            schedulerModel.Timeline.AddCommand(
                startObj: gameModel.ElapsedSeconds,
                digitalCommand: command);
        }

    }
}
