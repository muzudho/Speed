namespace Assets.Scripts.Vision.Models.Input
{
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Behaviours;
    using System;
    using UnityEngine;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfInputOfPlayer = Assets.Scripts.Vision.Models.Input.Players;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
    using ModelOfSchedulerO7thTimeline = Assets.Scripts.Vision.Models.Scheduler.O7thTimeline;
    using ModelOfThinkingEngine = Assets.Scripts.ThinkingEngine.Models;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;
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
            ModelOfInputOfPlayer.Meaning meaning)
        {
            this.PlayerIdObj = playerIdObj;
            this.NearCenterStackPlace = nearCenterStackPlace;
            this.FarCenterStackPlace = farCenterStackPlace;
            this.Meaning = meaning;
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
        internal ModelOfInputOfPlayer.Meaning Meaning { get; private set; }

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
        internal void Translate(
            ModelOfGame.Default gameModel)
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
                        playerObj: this.PlayerIdObj,
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
            ModelOfGame.Default gameModel,
            StalemateManager stalemateManager,
            ModelOfSchedulerO7thTimeline.Model timeline)
        {
            if (!this.Rights.IsHandled() &&
                !stalemateManager.IsStalemate &&
                this.Meaning.MoveCardToCenterStack(nearOrFarOfCenterStack) &&
                LegalMove.CanPutToCenterStack(gameModel, this.PlayerIdObj, gameModel.GetIndexOfFocusedCardOfPlayer(this.PlayerIdObj), this.GetCenterStackPlace(nearOrFarOfCenterStack)))
            {
                // ピックアップ中の場札を抜いて、台札へ積み上げる
                var command = new ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand(
                    playerObj: this.PlayerIdObj,
                    placeObj: this.GetCenterStackPlace(nearOrFarOfCenterStack));

                this.Rights.TimeOfRestObj = ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType());
                timeline.AddCommand(
                    startObj: gameModel.ElapsedSeconds,
                    command: command);
                this.Rights.IsThrowingCardIntoCenterStack = true;
            }
        }

        /// <summary>
        /// 左隣のカードをピックアップするように変えます
        /// </summary>
        internal void PickupCardToBackward(
            ModelOfGame.Default gameModel,
            StalemateManager stalemateManager,
            ModelOfSchedulerO7thTimeline.Model timeline)
        {
            // 制約：
            //      場札が２枚以上あるときに限る
            if (2 <= gameModel.GetCardsOfPlayerHand(this.PlayerIdObj).Count)
            {
                var command = new ModelOfThinkingEngineCommand.MoveFocusToNextCard(
                    playerObj: this.PlayerIdObj,
                    directionObj: ScriptOfThinkingEngine.Commons.PickLeft);

                this.Rights.TimeOfRestObj = ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType());
                timeline.AddCommand(
                    startObj: gameModel.ElapsedSeconds,
                    command: command);
            }
        }

        /// <summary>
        /// 右隣のカードをピックアップするように変えます
        /// </summary>
        internal void PickupCardToForward(
            ModelOfGame.Default gameModel,
            StalemateManager stalemateManager,
            ModelOfSchedulerO7thTimeline.Model timeline)
        {
            // 制約：
            //      場札が２枚以上あるときに限る
            if (2 <= gameModel.GetCardsOfPlayerHand(this.PlayerIdObj).Count)
            {
                var command = new ModelOfThinkingEngineCommand.MoveFocusToNextCard(
                    playerObj: this.PlayerIdObj,
                    directionObj: ScriptOfThinkingEngine.Commons.PickRight);

                this.Rights.TimeOfRestObj = ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType());
                timeline.AddCommand(
                    startObj: gameModel.ElapsedSeconds,
                    command: command);
            }
        }

        /// <summary>
        /// 手札を引く
        /// </summary>
        internal void DrawingHandCard(
            ModelOfGame.Default gameModel,
            ModelOfSchedulerO7thTimeline.Model timeline)
        {
            // 場札を並べる
            var command = new ModelOfThinkingEngineCommand.MoveCardsToHandFromPile(
                playerObj: this.PlayerIdObj,
                numberOfCards: 1);

            this.Rights.TimeOfRestObj = ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType());
            timeline.AddCommand(
                startObj: gameModel.ElapsedSeconds,
                command: command);
        }

    }
}
