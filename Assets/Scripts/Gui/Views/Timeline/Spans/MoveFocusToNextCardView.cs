namespace Assets.Scripts.Views.Timeline.Spans
{
    using Assets.Scripts.Simulators;
    using Assets.Scripts.ThikningEngine;
    using Assets.Scripts.ThikningEngine.CommandArgs;
    using Assets.Scripts.Views;
    using Assets.Scripts.Views.Moves;
    using System;
    using SimulatorsOfTimeline = Assets.Scripts.Simulators;

    /// <summary>
    /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
    /// </summary>
    class MoveFocusToNextCardView : AbstractSpanView
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override ISpanView Spawn()
        {
            return new MoveFocusToNextCardView();
        }

        // - プロパティ

        MoveFocusToNextCardModel GetModel(SimulatorsOfTimeline.TimeSpan timeSpan)
        {
            return (MoveFocusToNextCardModel)timeSpan.SpanModel;
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
            SimulatorsOfTimeline.TimeSpan timeSpan,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<SpanToLerp> setViewMovement)
        {
            GameModel gameModel = new GameModel(gameModelBuffer);
            int indexOfPrevious = gameModelBuffer.IndexOfFocusedCardOfPlayers[GetModel(timeSpan).Player]; // 下ろす場札

            int indexOfCurrent; // ピックアップする場札
            var length = gameModelBuffer.IdOfCardsOfPlayersHand[GetModel(timeSpan).Player].Count;

            if (length < 1)
            {
                // 場札が無いなら、何もピックアップされていません
                indexOfCurrent = -1;
            }
            else
            {
                switch (GetModel(timeSpan).Direction)
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
                var idOfCard = gameModel.GetCardAtOfPlayerHand(GetModel(timeSpan).Player, indexOfPrevious); // ピックアップしている場札

                // 前にフォーカスしていたカードを、盤に下ろす
                setViewMovement(MoveToDropHandCard.Generate(
                    startSeconds: timeSpan.StartSeconds,
                    duration: timeSpan.Duration,
                    idOfCard: idOfCard));
            }

            // モデル更新：ピックアップしている場札の、インデックス更新
            gameModelBuffer.IndexOfFocusedCardOfPlayers[GetModel(timeSpan).Player] = indexOfCurrent;

            if (0 <= indexOfCurrent && indexOfCurrent < length) // 範囲内なら
            {
                var idOfCard = gameModel.GetCardAtOfPlayerHand(GetModel(timeSpan).Player, indexOfCurrent); // ピックアップしている場札
                var idOfGo = Specification.GetIdOfGameObject(idOfCard);

                // 今回フォーカスするカードを持ち上げる
                setViewMovement(MoveToPickupHandCard.Generate(
                    startSeconds: timeSpan.StartSeconds,
                    duration: timeSpan.Duration,
                    idOfCard: idOfCard,
                    getBegin: () => new PositionAndRotationLazy(
                        getPosition: () => GameObjectStorage.Items[idOfGo].transform.position,
                        getRotation: () => GameObjectStorage.Items[idOfGo].transform.rotation)));
            }
        }
    }
}
