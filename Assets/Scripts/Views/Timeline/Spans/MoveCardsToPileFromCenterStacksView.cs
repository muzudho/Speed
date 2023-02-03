namespace Assets.Scripts.Views.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Spans;
    using Assets.Scripts.Views;
    using System;
    using UnityEngine;

    /// <summary>
    /// 右（または左）側の台札１枚を、手札へ移動する
    /// </summary>
    class MoveCardsToPileFromCenterStacksView : AbstractSpanModel
    {
        // - 生成

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="place"></param>
        internal MoveCardsToPileFromCenterStacksView(float startSeconds, float duration, MoveCardsToPileFromCenterStacksModel model)
            :base(startSeconds, duration)
        {
            this.Model = model;
        }

        // - プロパティ

        MoveCardsToPileFromCenterStacksModel Model { get; set; }

        // - メソッド

        /// <summary>
        /// 台札を、手札へ移動する
        /// 
        /// - ゲーム開始時に使う
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        public override void OnEnter(
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<CardMovementViewModel> setCardMovementModel)
        {
            // 台札の一番上（一番後ろ）のカードを１枚抜く
            var numberOfCards = 1;
            var length = gameModelBuffer.IdOfCardsOfCenterStacks[this.Model.Place].Count; // 台札の枚数
            if (1 <= length)
            {
                var startIndex = length - numberOfCards;
                var idOfCard = gameModelBuffer.IdOfCardsOfCenterStacks[this.Model.Place][startIndex];
                gameModelBuffer.RemoveCardAtOfCenterStack(this.Model.Place, startIndex);

                // 黒いカードは１プレイヤー、赤いカードは２プレイヤー
                int player;
                float angleY;
                var suit = idOfCard.Suit();
                switch (suit)
                {
                    case IdOfCardSuits.Clubs:
                    case IdOfCardSuits.Spades:
                        player = 0;
                        angleY = 180.0f;
                        break;

                    case IdOfCardSuits.Diamonds:
                    case IdOfCardSuits.Hearts:
                        player = 1;
                        angleY = 0.0f;
                        break;

                    default:
                        throw new Exception();
                }

                // プレイヤーの手札を積み上げる
                gameModelBuffer.AddCardOfPlayersPile(player, idOfCard);

                var goCard = GameObjectStorage.PlayingCards[idOfCard]; // TODO ビューから座標を取るしかない？
                setCardMovementModel(new CardMovementViewModel(
                    startSeconds: this.StartSeconds,
                    duration: this.Duration,
                    beginPosition: goCard.transform.position,
                    endPosition: new Vector3(gameViewModel.pileCardsX[player], gameViewModel.pileCardsY[player], gameViewModel.pileCardsZ[player]),
                    beginRotation: goCard.transform.rotation,
                    endRotation: Quaternion.Euler(0, angleY, 180.0f),
                    idOfCard: idOfCard));

                // 更新
                gameViewModel.pileCardsY[player] += 0.2f;
            }
        }
    }
}
