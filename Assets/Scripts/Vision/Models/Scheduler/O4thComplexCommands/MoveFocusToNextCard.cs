namespace Assets.Scripts.Vision.Models.Scheduler.O4thComplexCommands
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Models.World;
    using System;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;
    using ModelOfGameWriter = Assets.Scripts.ThinkingEngine.Models.Game.Writer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfSchedulerO3rdSimplexCommand = Assets.Scripts.Vision.Models.Scheduler.O3rdSimplexCommands;
    using ModelOfSchedulerO4thCommand = Assets.Scripts.Vision.Models.Scheduler.O4thComplexCommands;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    /// <summary>
    /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
    /// </summary>
    class MoveFocusToNextCard : ItsAbstract
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startObj"></param>
        /// <param name="command"></param>
        public MoveFocusToNextCard(
            GameSeconds startObj,
            ModelOfThinkingEngineCommand.IModel command)
            : base(startObj, command)
        {
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// 
        /// - ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
        /// </summary>
        public override void GenerateSpan(
            ModelOfGameBuffer.Model gameModelBuffer,
            ModelOfGameWriter.Model gameModelWriter,
            ModelOfInput.Init inputModel,
            ModelOfScheduler.Model schedulerModel,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimespan)
        {
            var command = (ModelOfThinkingEngineCommand.MoveFocusToNextCard)this.CommandOfThinkingEngine;

            // TODO 前のカードは、ピックアップしているという前提
            //
            // - 台札へ投げた直後は、前のカードはピックアップしていない
            //
            var previousFocusedHandCardObj = gameModelBuffer.GetPlayer(command.PlayerObj).FocusedHandCardObj; // 下ろす場札

            FocusedHandCard currentFocusedHandCardObj; // ピックアップする場札
            var length = gameModelBuffer.GetPlayer(command.PlayerObj).IdOfCardsOfHand.Count;

            if (length < 1)
            {
                // 場札が無いなら、何もピックアップされていません
                currentFocusedHandCardObj = FocusedHandCard.Empty;
            }
            else
            {
                if (command.DirectionObj == Commons.PickRight)
                {
                    if (previousFocusedHandCardObj.Index == HandCardIndex.Empty || length <= previousFocusedHandCardObj.Index.AsInt + 1)
                    {
                        // （ピックアップしているカードが無いとき）先頭の外から、先頭へ入ってくる
                        currentFocusedHandCardObj = FocusedHandCard.PickupFirst;
                    }
                    else
                    {
                        currentFocusedHandCardObj = new FocusedHandCard(true, new HandCardIndex(previousFocusedHandCardObj.Index.AsInt + 1));
                    }
                }
                else if (command.DirectionObj == Commons.PickLeft)
                {
                    if (previousFocusedHandCardObj.Index.AsInt - 1 < 0)
                    {
                        // （ピックアップしているカードが無いとき）最後尾の外から、最後尾へ入ってくる
                        currentFocusedHandCardObj = new FocusedHandCard(true, new HandCardIndex(length - 1));
                    }
                    else
                    {
                        currentFocusedHandCardObj = new FocusedHandCard(true, new HandCardIndex(previousFocusedHandCardObj.Index.AsInt - 1));
                    }
                }
                else
                {
                    throw new Exception();
                }
            }

            if (
                // インデックスが範囲内であり、
                HandCardIndex.First <= previousFocusedHandCardObj.Index && previousFocusedHandCardObj.Index.AsInt < length &&
                // ピックアップされている状態なら
                previousFocusedHandCardObj.IsPickUp)
            {
                var idOfCard = gameModelWriter.GetPlayer(command.PlayerObj).GetCardAtOfHand(previousFocusedHandCardObj.Index); // ピックアップしている場札

                // 前にピックアップしていたカードを、盤に下ろす
                setTimespan(ModelOfSchedulerO3rdSimplexCommand.DropHandCard.GenerateSpan(
                    timeRange: this.TimeRangeObj,
                    idOfCard: idOfCard));
            }

            // モデル更新：ピックアップしている場札の、インデックス更新
            gameModelWriter.GetPlayer(command.PlayerObj).UpdateFocusedHandCardObj(currentFocusedHandCardObj);

            if (HandCardIndex.First <= currentFocusedHandCardObj.Index && currentFocusedHandCardObj.Index.AsInt < length) // 範囲内なら
            {
                var idOfCard = gameModelWriter.GetPlayer(command.PlayerObj).GetCardAtOfHand(currentFocusedHandCardObj.Index); // ピックアップしている場札
                var idOfGo = IdMapping.GetIdOfGameObject(idOfCard);

                // 今回フォーカスするカードを持ち上げる
                setTimespan(ModelOfSchedulerO3rdSimplexCommand.PickupHandCard.GenerateSpan(
                    timeRange: this.TimeRangeObj,
                    idOfCard: idOfCard,
                    getBegin: () => new PositionAndRotationLazy(
                        getPosition: () => GameObjectStorage.Items[idOfGo].transform.position,
                        getRotation: () => GameObjectStorage.Items[idOfGo].transform.rotation),
                    onProgressOrNull: (progress) =>
                    {
                        if (1.0f <= progress)
                        {
                            // 制約の解除
                            inputModel.Players[command.PlayerObj.AsInt].Rights.IsPickupCartToNext = false;
                        }
                    }));
            }
            else
            {
                // 制約の解除
                inputModel.Players[command.PlayerObj.AsInt].Rights.IsPickupCartToNext = false;
            }
        }

        ModelOfThinkingEngineCommand.MoveFocusToNextCard GetCommandOfThinkingEngine(
            ModelOfSchedulerO4thCommand.IModel commandOfScheduler)
        {
            return (ModelOfThinkingEngineCommand.MoveFocusToNextCard)commandOfScheduler.CommandOfThinkingEngine;
        }
    }
}

