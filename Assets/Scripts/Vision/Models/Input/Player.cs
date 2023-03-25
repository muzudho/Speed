namespace Assets.Scripts.Vision.Models.Input
{
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Behaviours;
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
        /// もう入力できないなら真
        /// 
        /// - 編集可
        /// </summary>
        internal bool Handled { get; set; }

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

        /// <summary>
        /// 自分に近い方の台札
        /// </summary>
        internal ModelOfThinkingEngine.CenterStackPlace NearCenterStackPlace { get; private set; }

        /// <summary>
        /// 自分から遠い方の台札
        /// </summary>
        internal ModelOfThinkingEngine.CenterStackPlace FarCenterStackPlace { get; private set; }

        // - メソッド

        /// <summary>
        /// 入力を翻訳
        /// </summary>
        internal void Translate(
            ModelOfGame.Default gameModel)
        {
            // キー入力の解析：クリアー
            this.Meaning.Clear();

            // 前判定：もう入力できないなら真
            //
            // - スパム中
            // - 対局停止中
            this.Handled = 0.0f < this.Rights.TimeOfRestObj.AsFloat || !gameModel.IsGameActive;

            if (!this.Handled)
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
        /// 自分に近い方の台札へ置く
        /// </summary>
        internal void MoveCardToNearCenterStackFromHand(
            ModelOfGame.Default gameModel,
            StalemateManager stalemateManager,
            ModelOfSchedulerO7thTimeline.Model timeline)
        {
            if (!this.Handled &&
                !stalemateManager.IsStalemate &&
                this.Meaning.MoveCardToCenterStackNearMe &&
                LegalMove.CanPutToCenterStack(gameModel, this.PlayerIdObj, gameModel.GetIndexOfFocusedCardOfPlayer(this.PlayerIdObj), this.NearCenterStackPlace))
            {
                // ピックアップ中の場札を抜いて、台札へ積み上げる
                var command = new ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand(
                    playerObj: this.PlayerIdObj,
                    placeObj: this.NearCenterStackPlace);

                this.Rights.TimeOfRestObj = ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType());
                timeline.AddCommand(
                    startObj: gameModel.ElapsedSeconds,
                    command: command);
                this.Handled = true;
            }
        }

        /// <summary>
        /// 自分から遠い方の台札へ置く
        /// </summary>
        internal void MoveCardToFarCenterStackFromHand(
            ModelOfGame.Default gameModel,
            StalemateManager stalemateManager,
            ModelOfSchedulerO7thTimeline.Model timeline)
        {
            if (!this.Handled &&
                !stalemateManager.IsStalemate &&
                this.Meaning.MoveCardToFarCenterStack &&
                LegalMove.CanPutToCenterStack(gameModel, this.PlayerIdObj, gameModel.GetIndexOfFocusedCardOfPlayer(this.PlayerIdObj), this.FarCenterStackPlace))
            {
                // ピックアップ中の場札を抜いて、台札へ積み上げる
                var command = new ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand(
                    playerObj: this.PlayerIdObj,
                    placeObj: this.FarCenterStackPlace);

                this.Rights.TimeOfRestObj = ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType());
                timeline.AddCommand(
                    startObj: gameModel.ElapsedSeconds,
                    command: command);
                this.Handled = true;
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
    }
}
