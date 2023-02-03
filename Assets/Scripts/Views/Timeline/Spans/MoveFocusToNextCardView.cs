namespace Assets.Scripts.Views.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Spans;
    using Assets.Scripts.Views;
    using System;
    using SimulatorsOfTimeline = Assets.Scripts.Simulators.Timeline;

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
        public override ISpanView Spawn(SimulatorsOfTimeline.TimeSpan timeSpan)
        {
            return new MoveFocusToNextCardView(timeSpan);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSpan">タイム・スパン</param>
        internal MoveFocusToNextCardView(SimulatorsOfTimeline.TimeSpan timeSpan)
            : base(timeSpan)
        {
        }

        // - プロパティ

        MoveFocusToNextCardModel Model
        {
            get
            {
                return (MoveFocusToNextCardModel)this.TimeSpan.SpanModel;
            }
        }

        // - メソッド

        /// <summary>
        /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
        /// </summary>
        /// <param name="player"></param>
        /// <param name="direction">後ろ:0, 前:1</param>
        public override void OnEnter(
            SimulatorsOfTimeline.TimeSpan timeSpan,
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<MovementViewModel> setMovementViewModel)
        {
            GameModel gameModel = new GameModel(gameModelBuffer);
            int indexOfPrevious = gameModelBuffer.IndexOfFocusedCardOfPlayers[this.Model.Player]; // 下ろす場札

            int indexOfCurrent; // ピックアップする場札
            var length = gameModelBuffer.IdOfCardsOfPlayersHand[this.Model.Player].Count;

            if (length < 1)
            {
                // 場札が無いなら、何もピックアップされていません
                indexOfCurrent = -1;
            }
            else
            {
                switch (this.Model.Direction)
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
                var idOfCard = gameModel.GetCardAtOfPlayerHand(this.Model.Player, indexOfPrevious); // ピックアップしている場札

                // 前にフォーカスしていたカードを、盤に下ろす
                setMovementViewModel(MovementGenerator.PutDownCardOfHand(
                    startSeconds: this.TimeSpan.StartSeconds,
                    duration: this.TimeSpan.Duration,
                    idOfCard: idOfCard));
            }

            // （状態変更）ピックアップしている場札の、インデックス更新
            this.Model.SetIndexOfNextFocusedHandCard(indexOfCurrent);

            if (0 <= indexOfCurrent && indexOfCurrent < length) // 範囲内なら
            {
                var idOfCard = gameModel.GetCardAtOfPlayerHand(this.Model.Player, indexOfCurrent); // ピックアップしている場札

                // 今回フォーカスするカードを持ち上げる
                setMovementViewModel(MovementGenerator.PickupCardOfHand(
                    startSeconds: this.TimeSpan.StartSeconds,
                    duration: this.TimeSpan.Duration,
                    idOfCard: idOfCard));
            }
        }
    }
}
