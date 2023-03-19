namespace Assets.Scripts.Vision.Models.Timeline.O4thGameOperation
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using Assets.Scripts.Vision.Models.World;
    using System;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfTimelineO1stSpan = Assets.Scripts.Vision.Models.Timeline.O1stSpan;
    using ModelOfTimelineO3rdSpanGenerator = Assets.Scripts.Vision.Models.Timeline.O3rdSpanGenerator;

    /// <summary>
    /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
    /// </summary>
    class MoveFocusToNextCardView : ItsAbstract
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override IModel NewThis()
        {
            return new MoveFocusToNextCardView();
        }

        // - プロパティ

        MoveFocusToNextCardModel GetModel(ITimedGenerator timedGenerator)
        {
            return (MoveFocusToNextCardModel)timedGenerator.TimedCommandArg.CommandArg;
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// 
        /// - ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
        /// </summary>
        /// <param name="player"></param>
        /// <param name="direction">後ろ:0, 前:1</param>
        public override void CreateSpan(
            ITimedGenerator timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfTimelineO1stSpan.IBasecaseSpan> setViewMovement)
        {
            ModelOfGame.Default gameModel = new ModelOfGame.Default(gameModelBuffer);
            var indexOfPreviousObj = gameModelBuffer.IndexOfFocusedCardOfPlayersObj[GetModel(timedGenerator).PlayerObj.AsInt]; // 下ろす場札

            HandCardIndex indexOfCurrentObj; // ピックアップする場札
            var length = gameModelBuffer.IdOfCardsOfPlayersHand[GetModel(timedGenerator).PlayerObj.AsInt].Count;

            if (length < 1)
            {
                // 場札が無いなら、何もピックアップされていません
                indexOfCurrentObj = Commons.HandCardIndexNoSelected;
            }
            else
            {
                if (GetModel(timedGenerator).DirectionObj == Commons.PickRight)
                {
                    if (indexOfPreviousObj == Commons.HandCardIndexNoSelected || length <= indexOfPreviousObj.AsInt + 1)
                    {
                        // （ピックアップしているカードが無いとき）先頭の外から、先頭へ入ってくる
                        indexOfCurrentObj = Commons.HandCardIndexFirst;
                    }
                    else
                    {
                        indexOfCurrentObj = new HandCardIndex(indexOfPreviousObj.AsInt + 1);
                    }
                }
                else if (GetModel(timedGenerator).DirectionObj == Commons.PickLeft)
                {
                    if (indexOfPreviousObj.AsInt - 1 < 0)
                    {
                        // （ピックアップしているカードが無いとき）最後尾の外から、最後尾へ入ってくる
                        indexOfCurrentObj = new HandCardIndex(length - 1);
                    }
                    else
                    {
                        indexOfCurrentObj = new HandCardIndex(indexOfPreviousObj.AsInt - 1);
                    }
                }
                else
                {
                    throw new Exception();
                }
            }

            if (Commons.HandCardIndexFirst <= indexOfPreviousObj && indexOfPreviousObj.AsInt < length) // 範囲内なら
            {
                var idOfCard = gameModel.GetCardAtOfPlayerHand(GetModel(timedGenerator).PlayerObj, indexOfPreviousObj); // ピックアップしている場札

                // 前にフォーカスしていたカードを、盤に下ろす
                setViewMovement(ModelOfTimelineO3rdSpanGenerator.DropHandCard.GenerateSpan(
                    startSeconds: timedGenerator.StartSeconds,
                    duration: timedGenerator.TimedCommandArg.Duration,
                    idOfCard: idOfCard));
            }

            // モデル更新：ピックアップしている場札の、インデックス更新
            gameModelBuffer.IndexOfFocusedCardOfPlayersObj[GetModel(timedGenerator).PlayerObj.AsInt] = indexOfCurrentObj;

            if (Commons.HandCardIndexFirst <= indexOfCurrentObj && indexOfCurrentObj.AsInt < length) // 範囲内なら
            {
                var idOfCard = gameModel.GetCardAtOfPlayerHand(GetModel(timedGenerator).PlayerObj, indexOfCurrentObj); // ピックアップしている場札
                var idOfGo = IdMapping.GetIdOfGameObject(idOfCard);

                // 今回フォーカスするカードを持ち上げる
                setViewMovement(ModelOfTimelineO3rdSpanGenerator.PickupHandCard.GenerateSpan(
                    startSeconds: timedGenerator.StartSeconds,
                    duration: timedGenerator.TimedCommandArg.Duration,
                    idOfCard: idOfCard,
                    getBegin: () => new PositionAndRotationLazy(
                        getPosition: () => GameObjectStorage.Items[idOfGo].transform.position,
                        getRotation: () => GameObjectStorage.Items[idOfGo].transform.rotation)));
            }
        }
    }
}
