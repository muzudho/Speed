namespace Assets.Scripts.Models.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline;
    using Assets.Scripts.Views;
    using System;
    using UnityEngine;

    /// <summary>
    /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
    /// </summary>
    class MoveFocusToNextCard : AbstractSpan
    {
        // - 生成

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="player"></param>
        /// <param name="direction"></param>
        /// <param name="setIndexOfNextFocusedHandCard"></param>
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

        #region Lerpに使うもの
        /// <summary>
        /// カードを持ち上げる動き
        /// </summary>
        CardMovementModel CardUp { get; set; }

        /// <summary>
        /// カードを置く動き
        /// </summary>
        CardMovementModel CardDown { get; set; }
        #endregion

        // - メソッド

        /// <summary>
        /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
        /// </summary>
        /// <param name="player"></param>
        /// <param name="direction">後ろ:0, 前:1</param>
        public override void OnEnter(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
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
                this.CardDown = MovementGenerator.PutDownCardOfHand(
                    startSeconds: this.StartSeconds,
                    duration: this.Duration,
                    gameModel: gameModel,
                    player: Player,
                    handIndex: indexOfFocusedHandCard);
            }

            if (0 <= current && current < gameModelBuffer.IdOfCardsOfPlayersHand[Player].Count) // 範囲内なら
            {
                // 今回フォーカスするカードを持ち上げる
                this.CardUp = MovementGenerator.PickupCardOfHand(
                    startSeconds: this.StartSeconds,
                    duration: this.Duration,
                    gameModel: gameModel,
                    player: Player,
                    handIndex: current);
            }
        }

        public override void Lerp(float progress)
        {
            base.Lerp(progress);

            // カードを置く動き
            this.CardDown.GameObject.transform.position = Vector3.Lerp(this.CardDown.BeginPosition, this.CardDown.EndPosition, progress);
            this.CardDown.GameObject.transform.rotation = Quaternion.Lerp(this.CardDown.BeginRotation, this.CardDown.EndRotation, progress);

            // カードを持ち上げる動き
            this.CardUp.GameObject.transform.position = Vector3.Lerp(this.CardUp.BeginPosition, this.CardUp.EndPosition, progress);
            this.CardUp.GameObject.transform.rotation = Quaternion.Lerp(this.CardUp.BeginRotation, this.CardUp.EndRotation, progress);
        }

        public override void OnLeave()
        {
            base.OnLeave();

            // TODO ★★ 動作が完了する前に、次の動作を行うと、カードがどんどん沈んでいく、といったことが起こる。連打スパム対策が必要

            // カードを置く動き（完了）
            this.CardDown.GameObject.transform.position = this.CardDown.EndPosition;
            this.CardDown.GameObject.transform.rotation = this.CardDown.EndRotation;

            // カードを持ち上げる動き（完了）
            this.CardUp.GameObject.transform.position = this.CardUp.EndPosition;
            this.CardUp.GameObject.transform.rotation = this.CardUp.EndRotation;
        }
    }
}
