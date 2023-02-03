﻿namespace Assets.Scripts.Views.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Spans;
    using Assets.Scripts.Simulators.Timeline;
    using Assets.Scripts.Views;
    using System;
    using UnityEngine;
    using SimulatorsOfTimeline = Assets.Scripts.Simulators.Timeline;

    /// <summary>
    /// 右（または左）側の台札１枚を、手札へ移動する
    /// </summary>
    class MoveCardsToPileFromCenterStacksView : AbstractSpanView
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override ISpanView Spawn()
        {
            return new MoveCardsToPileFromCenterStacksView();
        }

        // - プロパティ

        MoveCardsToPileFromCenterStacksModel GetModel(SimulatorsOfTimeline.TimeSpan timeSpan)
        {
            return (MoveCardsToPileFromCenterStacksModel)timeSpan.SpanModel;
        }

        // - メソッド

        /// <summary>
        /// 台札を、手札へ移動する
        /// 
        /// - ゲーム開始時に使う
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        public override void OnEnter(
            SimulatorsOfTimeline.TimeSpan timeSpan,
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<MovementViewModel> setMovementViewModel)
        {
            // 台札の一番上（一番後ろ）のカードを１枚抜く
            var numberOfCards = 1;
            var length = gameModelBuffer.IdOfCardsOfCenterStacks[GetModel(timeSpan).Place].Count; // 台札の枚数
            if (1 <= length)
            {
                var startIndex = length - numberOfCards;
                var idOfCard = gameModelBuffer.IdOfCardsOfCenterStacks[GetModel(timeSpan).Place][startIndex];
                gameModelBuffer.RemoveCardAtOfCenterStack(GetModel(timeSpan).Place, startIndex);

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

                var idOfGo = Specification.GetIdOfGameObject(idOfCard);
                var goCard = GameObjectStorage.Items[idOfGo]; // TODO ビューから座標を取るしかない？
                setMovementViewModel(new MovementViewModel(
                    startSeconds: timeSpan.StartSeconds,
                    duration: timeSpan.Duration,
                    getBeginPosition: ()=>goCard.transform.position,
                    getEndPosition:()=> new Vector3(gameViewModel.pileCardsX[player], gameViewModel.pileCardsY[player], gameViewModel.pileCardsZ[player]),
                    beginRotation: goCard.transform.rotation,
                    endRotation: Quaternion.Euler(0, angleY, 180.0f),
                    idOfGameObject: idOfGo));

                // 更新
                gameViewModel.pileCardsY[player] += 0.2f;
            }
        }
    }
}
