namespace Assets.Scripts.Models.Timeline.Commands
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Motions;
    using Assets.Scripts.Views;
    using System;
    using UnityEngine;

    /// <summary>
    /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
    /// </summary>
    class MoveFocusToNextCard : AbstractCommand
    {
        // - 生成

        internal MoveFocusToNextCard(int player, int direction, LazyArgs.SetValue<int> setIndexOfNextFocusedHandCard)
        {
            this.Player = player;
            this.Direction = direction;
            this.SetIndexOfNextFocusedHandCard = setIndexOfNextFocusedHandCard;
        }

        // - プロパティ

        int Player { get; set; }
        int Direction { get; set; }
        LazyArgs.SetValue<int> SetIndexOfNextFocusedHandCard { get; set; }

        #region Leapに使うもの
        /// <summary>
        /// カードを持ち上げる動き
        /// </summary>
        CardMovement CardUp { get; set; }

        /// <summary>
        /// カードを置く動き
        /// </summary>
        CardMovement CardDown { get; set; }
        #endregion

        // - メソッド

        /// <summary>
        /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
        /// </summary>
        /// <param name="player"></param>
        /// <param name="direction">後ろ:0, 前:1</param>
        public override void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            GameModel gameModel = new GameModel(gameModelBuffer);
            int indexOfFocusedHandCard = gameModelBuffer.IndexOfFocusedCardOfPlayers[Player];

            int current;
            var length = gameModelBuffer.IdOfCardsOfPlayersHand[Player].Count;

            if (length < 1)
            {
                // 場札が無いなら、何もピックアップされていません
                current = -1;
            }
            else
            {
                switch (Direction)
                {
                    // 後ろへ
                    case 0:
                        if (indexOfFocusedHandCard == -1 || length <= indexOfFocusedHandCard + 1)
                        {
                            // （ピックアップしているカードが無いとき）先頭の外から、先頭へ入ってくる
                            current = 0;
                        }
                        else
                        {
                            current = indexOfFocusedHandCard + 1;
                        }
                        break;

                    // 前へ
                    case 1:
                        if (indexOfFocusedHandCard == -1 || indexOfFocusedHandCard - 1 < 0)
                        {
                            // （ピックアップしているカードが無いとき）最後尾の外から、最後尾へ入ってくる
                            current = length - 1;
                        }
                        else
                        {
                            current = indexOfFocusedHandCard - 1;
                        }
                        break;

                    default:
                        throw new Exception();
                }
            }

            SetIndexOfNextFocusedHandCard(current);

            if (0 <= indexOfFocusedHandCard && indexOfFocusedHandCard < gameModelBuffer.IdOfCardsOfPlayersHand[Player].Count) // 範囲内なら
            {
                // 前にフォーカスしていたカードを、盤に下ろす
                gameViewModel.PutDownCardOfHand(
                    gameModel: gameModel,
                    player: Player,
                    handIndex: indexOfFocusedHandCard,
                    setResults: (results) =>
                    {
                        this.CardDown = new CardMovement(
                            beginPosition: results.Item1,
                            endPosition: results.Item2,
                            beginRotation: results.Item3,
                            endRotation: results.Item4,
                            goCard: results.Item5);
                    });
            }

            if (0 <= current && current < gameModelBuffer.IdOfCardsOfPlayersHand[Player].Count) // 範囲内なら
            {
                // 今回フォーカスするカードを持ち上げる
                gameViewModel.PickupCardOfHand(
                    gameModel: gameModel,
                    player: Player,
                    handIndex: current,
                    setResults: (results) =>
                    {
                        this.CardUp = new CardMovement(
                            beginPosition: results.Item1,
                            endPosition: results.Item2,
                            beginRotation: results.Item3,
                            endRotation: results.Item4,
                            goCard: results.Item5);
                    });
            }
        }

        public override void Leap(float progress)
        {
            base.Leap(progress);

            // カードを置く動き
            this.CardDown.GoCard.transform.position = Vector3.Lerp(this.CardDown.BeginPosition, this.CardDown.EndPosition, progress);
            this.CardDown.GoCard.transform.rotation = Quaternion.Lerp(this.CardDown.BeginRotation, this.CardDown.EndRotation, progress);

            // カードを持ち上げる動き
            this.CardUp.GoCard.transform.position = Vector3.Lerp(this.CardUp.BeginPosition, this.CardUp.EndPosition, progress);
            this.CardUp.GoCard.transform.rotation = Quaternion.Lerp(this.CardUp.BeginRotation, this.CardUp.EndRotation, progress);
        }

        public override void OnLeave()
        {
            base.OnLeave();

            // TODO ★★ 動作が完了する前に、次の動作を行うと、カードがどんどん沈んでいく、といったことが起こる。連打スパム対策が必要

            // カードを置く動き（完了）
            this.CardDown.GoCard.transform.position = this.CardDown.EndPosition;
            this.CardDown.GoCard.transform.rotation = this.CardDown.EndRotation;

            // カードを持ち上げる動き（完了）
            this.CardUp.GoCard.transform.position = this.CardUp.EndPosition;
            this.CardUp.GoCard.transform.rotation = this.CardUp.EndRotation;
        }
    }
}
