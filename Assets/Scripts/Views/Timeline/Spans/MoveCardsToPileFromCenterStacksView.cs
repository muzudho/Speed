﻿namespace Assets.Scripts.Views.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Spans;
    using Assets.Scripts.Simulators.Timeline;
    using Assets.Scripts.Views;
    using Assets.Scripts.Views.Moves;
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
        /// ゲーム画面の同期を始めます
        /// 
        /// - 台札を、手札へ移動する
        /// - ゲーム開始時に使う
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        public override void OnEnter(
            SimulatorsOfTimeline.TimeSpan timeSpan,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ViewMovement> setViewMovement)
        {
            // 台札の一番上（一番後ろ）のカードを１枚抜く
            var numberOfCards = 1;
            var length = gameModelBuffer.IdOfCardsOfCenterStacks[GetModel(timeSpan).Place].Count; // 台札の枚数
            if (1 <= length)
            {
                var startIndex = length - numberOfCards;
                var idOfCardOfCenterStack = gameModelBuffer.IdOfCardsOfCenterStacks[GetModel(timeSpan).Place][startIndex]; // 台札の１番上のカード
                gameModelBuffer.RemoveCardAtOfCenterStack(GetModel(timeSpan).Place, startIndex);

                // 黒いカードは１プレイヤー、赤いカードは２プレイヤー
                int player;
                float angleY;
                var suit = idOfCardOfCenterStack.Suit();
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
                gameModelBuffer.AddCardOfPlayersPile(player, idOfCardOfCenterStack);

                // 台札から手札へ移動するカードについて
                var idOfGameObjectOfCard = Specification.GetIdOfGameObject(idOfCardOfCenterStack);
                var lengthOfPile = gameModelBuffer.IdOfCardsOfPlayersPile[player].Count;
                var idOfTopOfPile = gameModelBuffer.IdOfCardsOfPlayersPile[player][lengthOfPile - 1]; // 手札の天辺

                Vector3? startPosition = null;
                Quaternion? startRotation = null;
                Vector3? endPosition = null;
                Quaternion? endRotation = null;

                setViewMovement(MoveToMoveCardsToPileFromCenterStacks.Generate(
                    startSeconds: timeSpan.StartSeconds,
                    duration: timeSpan.Duration,
                    target: idOfGameObjectOfCard,
                    getBegin: () => new PositionAndRotationLazy(
                        getPosition: () =>
                        {
                            // 初回アクセス時に、値固定
                            if (startPosition == null)
                            {
                                startPosition = GameObjectStorage.Items[idOfGameObjectOfCard].transform.position;
                            }
                            return startPosition ?? throw new Exception();
                        },
                        getRotation: () =>
                        {
                            // 初回アクセス時に、値固定
                            if (startRotation == null)
                            {
                                startRotation = GameObjectStorage.Items[idOfGameObjectOfCard].transform.rotation;
                            }
                            return startRotation ?? throw new Exception();
                        }),
                    getEnd: () => new PositionAndRotationLazy(
                        getPosition: () =>
                        {
                            // 初回アクセス時に、値固定
                            if (endPosition == null)
                            {
                                // 現在の天辺の手札のポジションより１枚分上、または、一番下
                                // 手札が１枚も無ければ
                                if (lengthOfPile < 1)
                                {
                                    // 一番下
                                    endPosition = GameView.positionOfPileCardsOrigin[player].ToMutable();
                                }
                                // 既存の手札があれば
                                else
                                {
                                    var goCardOfTop = GameObjectStorage.Items[Specification.GetIdOfGameObject(idOfTopOfPile)];
                                    // より、１枚分上
                                    endPosition = goCardOfTop.transform.position;
                                }
                            }
                            return endPosition ?? throw new Exception();
                        },
                        getRotation: () =>
                        {
                            // 初回アクセス時に、値固定
                            if (endRotation == null)
                            {
                                endRotation = Quaternion.Euler(0, angleY, 180.0f);
                            }
                            return endRotation ?? throw new Exception();
                        })
                    ));
            }
        }
    }
}
