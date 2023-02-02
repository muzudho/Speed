namespace Assets.Scripts.Models.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline;
    using Assets.Scripts.Views;
    using System;

    /// <summary>
    /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
    /// </summary>
    class MoveFocusToNextCard : AbstractSpanModel
    {
        // - 生成

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="player"></param>
        /// <param name="direction"></param>
        /// <param name="setIndexOfNextFocusedHandCard">次にピックアップする場札は何番目</param>
        internal MoveFocusToNextCard(float startSeconds, float duration, int player, int direction, LazyArgs.SetValue<int> setIndexOfNextFocusedHandCard)
            : base(startSeconds, duration)
        {
            this.Player = player;
            this.Direction = direction;
            this.SetIndexOfNextFocusedHandCard = setIndexOfNextFocusedHandCard;
        }

        // - プロパティ

        int Player { get; set; }
        int Direction { get; set; }
        LazyArgs.SetValue<int> SetIndexOfNextFocusedHandCard { get; set; }

        // - メソッド

        /// <summary>
        /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
        /// </summary>
        /// <param name="player"></param>
        /// <param name="direction">後ろ:0, 前:1</param>
        public override void OnEnter(
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<CardMovementModel> setCardMovementModel)
        {
            GameModel gameModel = new GameModel(gameModelBuffer);
            int indexOfPrevious = gameModelBuffer.IndexOfFocusedCardOfPlayers[Player]; // 下ろす場札

            int indexOfCurrent; // ピックアップする場札
            var length = gameModelBuffer.IdOfCardsOfPlayersHand[Player].Count;

            if (length < 1)
            {
                // 場札が無いなら、何もピックアップされていません
                indexOfCurrent = -1;
            }
            else
            {
                switch (Direction)
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
                // 前にフォーカスしていたカードを、盤に下ろす
                setCardMovementModel(MovementGenerator.PutDownCardOfHand(
                    startSeconds: this.StartSeconds,
                    duration: this.Duration,
                    gameModel: gameModel,
                    player: Player,
                    handIndex: indexOfPrevious));
            }

            // （状態変更）ピックアップしている場札の、インデックス更新
            SetIndexOfNextFocusedHandCard(indexOfCurrent);

            if (0 <= indexOfCurrent && indexOfCurrent < length) // 範囲内なら
            {
                // 今回フォーカスするカードを持ち上げる
                setCardMovementModel(MovementGenerator.PickupCardOfHand(
                    startSeconds: this.StartSeconds,
                    duration: this.Duration,
                    gameModel: gameModel,
                    player: Player,
                    handIndex: indexOfCurrent));
            }
        }
    }
}
