namespace Assets.Scripts.Gui.SpanOfLerp.GeneratorGenerator
{
    using Assets.Scripts.Gui.SpanOfLerp.Generator;
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.CommandArgs;
    using Assets.Scripts.Views;
    using System;
    using SimulatorsOfTimeline = Assets.Scripts.Gui.SpanOfLerp.TimedGenerator;
    using SpanOfLeap = Assets.Scripts.Gui.SpanOfLerp;

    /// <summary>
    /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
    /// </summary>
    class MoveFocusToNextCardView : AbstractSpanGenerator
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override ISpanGenerator Spawn()
        {
            return new MoveFocusToNextCardView();
        }

        // - プロパティ

        MoveFocusToNextCardModel GetModel(SimulatorsOfTimeline.TimedGenerator timedGenerator)
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
        public override void CreateSpanToLerp(
            SimulatorsOfTimeline.TimedGenerator timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<SpanOfLeap.Model> setViewMovement)
        {
            GameModel gameModel = new GameModel(gameModelBuffer);
            int indexOfPrevious = gameModelBuffer.IndexOfFocusedCardOfPlayers[GetModel(timedGenerator).Player]; // 下ろす場札

            int indexOfCurrent; // ピックアップする場札
            var length = gameModelBuffer.IdOfCardsOfPlayersHand[GetModel(timedGenerator).Player].Count;

            if (length < 1)
            {
                // 場札が無いなら、何もピックアップされていません
                indexOfCurrent = -1;
            }
            else
            {
                switch (GetModel(timedGenerator).Direction)
                {
                    // 後ろへ
                    case 0:
                        if (indexOfPrevious == -1 || length <= indexOfPrevious + 1)
                        {
                            // （ピックアップしているカードが無いとき）先頭の外から、先頭へ入ってくる
                            indexOfCurrent = 0;
                        }
                        else
                        {
                            indexOfCurrent = indexOfPrevious + 1;
                        }
                        break;

                    // 前へ
                    case 1:
                        if (indexOfPrevious - 1 < 0)
                        {
                            // （ピックアップしているカードが無いとき）最後尾の外から、最後尾へ入ってくる
                            indexOfCurrent = length - 1;
                        }
                        else
                        {
                            indexOfCurrent = indexOfPrevious - 1;
                        }
                        break;

                    default:
                        throw new Exception();
                }
            }


            if (0 <= indexOfPrevious && indexOfPrevious < length) // 範囲内なら
            {
                var idOfCard = gameModel.GetCardAtOfPlayerHand(GetModel(timedGenerator).Player, indexOfPrevious); // ピックアップしている場札

                // 前にフォーカスしていたカードを、盤に下ろす
                setViewMovement(DropHandCard.Generate(
                    startSeconds: timedGenerator.StartSeconds,
                    duration: timedGenerator.TimedCommandArg.Duration,
                    idOfCard: idOfCard));
            }

            // モデル更新：ピックアップしている場札の、インデックス更新
            gameModelBuffer.IndexOfFocusedCardOfPlayers[GetModel(timedGenerator).Player] = indexOfCurrent;

            if (0 <= indexOfCurrent && indexOfCurrent < length) // 範囲内なら
            {
                var idOfCard = gameModel.GetCardAtOfPlayerHand(GetModel(timedGenerator).Player, indexOfCurrent); // ピックアップしている場札
                var idOfGo = IdMapping.GetIdOfGameObject(idOfCard);

                // 今回フォーカスするカードを持ち上げる
                setViewMovement(PickupHandCard.Generate(
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
