namespace Assets.Scripts.Scheduler.AnalogCommands.O4thComplex
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Models;
    using Assets.Scripts.Vision.Models.World;
    using System;
    using System.Collections.Generic;
    using ModelOfAnalogCommand1stTimelineSpan = Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan;
    using ModelOfAnalogCommand3rdSimplex = Assets.Scripts.Scheduler.AnalogCommands.O3rdSimplex;
    using ModelOfAnalogCommand4thComplex = Assets.Scripts.Scheduler.AnalogCommands.O4thComplex;
    using ModelOfAnalogCommands = Assets.Scripts.Scheduler.AnalogCommands;
    using ModelOfDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;
    using ModelOfGameWriter = Assets.Scripts.ThinkingEngine.Models.Game.Writer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfThinkingEngineCommons = Assets.Scripts.ThinkingEngine.Commons;

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
        /// <param name="digitalCommand"></param>
        public MoveFocusToNextCard(
            GameSeconds startObj,
            ModelOfDigitalCommands.IModel digitalCommand)
            : base(startObj, digitalCommand)
        {
        }

        // - メソッド

        /// <summary>
        /// 準備
        /// </summary>
        public void Setup()
        {

        }

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// 
        /// - ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
        /// </summary>
        public override List<ModelOfAnalogCommand1stTimelineSpan.IModel> CreateTimespanList(
            ModelOfGameBuffer.Model gameModelBuffer,
            ModelOfGameWriter.Model gameModelWriter,
            ModelOfInput.Init inputModel,
            ModelOfAnalogCommands.Model schedulerModel)
        {
            var result = new List<ModelOfAnalogCommand1stTimelineSpan.IModel>();

            var digitalCommand = (ModelOfDigitalCommands.MoveFocusToNextCard)this.DigitalCommand;

            // TODO 前のカードは、ピックアップしているという前提
            //
            // - 台札へ投げた直後は、前のカードはピックアップしていない
            //
            var oldFocusedHandCardObj = gameModelBuffer.GetPlayer(digitalCommand.PlayerObj).FocusedHandCardObj; // 下ろす場札

            FocusedHandCard nextFocusedHandCardObj; // ピックアップする場札
            var length = gameModelBuffer.GetPlayer(digitalCommand.PlayerObj).IdOfCardsOfHand.Count;

            if (length < 1)
            {
                // 場札が無いなら、何もピックアップされていません
                nextFocusedHandCardObj = FocusedHandCard.Empty;
            }
            else
            {
                if (digitalCommand.DirectionObj == ModelOfThinkingEngineCommons.PickRight)
                {
                    if (oldFocusedHandCardObj.Index == HandCardIndex.Empty || length <= oldFocusedHandCardObj.Index.AsInt + 1)
                    {
                        // （ピックアップしているカードが無いか、最後尾のカードをピックアップしていたとき）先頭のカードをピックアップする
                        nextFocusedHandCardObj = FocusedHandCard.PickupFirst;
                    }
                    else
                    {
                        // （ピックアップしていたカードの）次のカードをピックアップする
                        nextFocusedHandCardObj = new FocusedHandCard(true, new HandCardIndex(oldFocusedHandCardObj.Index.AsInt + 1));
                    }
                }
                else if (digitalCommand.DirectionObj == ModelOfThinkingEngineCommons.PickLeft)
                {
                    if (oldFocusedHandCardObj.Index.AsInt < 1)
                    {
                        // （ピックアップしているカードが先頭だったとき）最後尾のカードをピックアップする
                        nextFocusedHandCardObj = new FocusedHandCard(true, new HandCardIndex(length - 1));
                    }
                    else
                    {
                        // （ピックアップしていたカードの）次のカードをピックアップする
                        nextFocusedHandCardObj = new FocusedHandCard(true, new HandCardIndex(oldFocusedHandCardObj.Index.AsInt - 1));
                    }
                }
                else
                {
                    // ここには来ない
                    throw new Exception();
                }
            }

            if (
                // インデックスが範囲内であり、
                HandCardIndex.First <= oldFocusedHandCardObj.Index && oldFocusedHandCardObj.Index.AsInt < length &&
                // ピックアップされている状態なら
                oldFocusedHandCardObj.IsPickUp)
            {
                var idOfCard = gameModelWriter.GetPlayer(digitalCommand.PlayerObj).GetCardAtOfHand(oldFocusedHandCardObj.Index); // ピックアップしている場札

                // 前にピックアップしていたカードを、盤に下ろす
                result.Add(ModelOfAnalogCommand3rdSimplex.DropHandCard.CreateTimespan(
                    timeRange: this.TimeRangeObj,
                    idOfCard: idOfCard));
            }

            // モデル更新：ピックアップしている場札の、インデックス更新
            gameModelWriter.GetPlayer(digitalCommand.PlayerObj).UpdateFocusedHandCardObj(nextFocusedHandCardObj);

            if (HandCardIndex.First <= nextFocusedHandCardObj.Index && nextFocusedHandCardObj.Index.AsInt < length) // 範囲内なら
            {
                var idOfCard = gameModelWriter.GetPlayer(digitalCommand.PlayerObj).GetCardAtOfHand(nextFocusedHandCardObj.Index); // ピックアップしている場札
                var idOfGo = IdMapping.GetIdOfGameObject(idOfCard);

                // 今回フォーカスするカードを持ち上げる
                result.Add(ModelOfAnalogCommand3rdSimplex.PickupHandCard.CreateTimespan(
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
                            inputModel.Players[digitalCommand.PlayerObj.AsInt].Rights.IsPickupCartToNext = false;
                        }
                    }));
            }
            else
            {
                // 制約の解除
                inputModel.Players[digitalCommand.PlayerObj.AsInt].Rights.IsPickupCartToNext = false;
            }

            return result;
        }

        ModelOfDigitalCommands.MoveFocusToNextCard GetCommandOfThinkingEngine(
            ModelOfAnalogCommand4thComplex.IModel analogCommand)
        {
            return (ModelOfDigitalCommands.MoveFocusToNextCard)analogCommand.DigitalCommand;
        }
    }
}

