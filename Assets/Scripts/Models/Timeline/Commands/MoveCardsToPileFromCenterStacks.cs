﻿namespace Assets.Scripts.Models.Timeline.Commands
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;
    using System;
    using UnityEngine;

    /// <summary>
    /// 右（または左）側の台札１枚を、手札へ移動する
    /// </summary>
    class MoveCardsToPileFromCenterStacks : AbstractCommand
    {
        // - 生成

        internal MoveCardsToPileFromCenterStacks(int place)
        {
            this.Place = place;
        }

        // - プロパティ

        int Place { get; set; }

        // - メソッド

        /// <summary>
        /// 台札を、手札へ移動する
        /// 
        /// - ゲーム開始時に使う
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        public override void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            // 台札の一番上（一番後ろ）のカードを１枚抜く
            var numberOfCards = 1;
            var length = gameModelBuffer.IdOfCardsOfCenterStacks[Place].Count; // 台札の枚数
            if (1 <= length)
            {
                var startIndex = length - numberOfCards;
                var idOfCard = gameModelBuffer.IdOfCardsOfCenterStacks[Place][startIndex];
                gameModelBuffer.RemoveCardAtOfCenterStack(Place, startIndex);

                // 黒いカードは１プレイヤー、赤いカードは２プレイヤー
                int player;
                float angleY;
                var goCard = GameObjectStorage.PlayingCards[idOfCard];
                if (goCard.name.StartsWith("Clubs") || goCard.name.StartsWith("Spades"))
                {
                    player = 0;
                    angleY = 180.0f;
                }
                else if (goCard.name.StartsWith("Diamonds") || goCard.name.StartsWith("Hearts"))
                {
                    player = 1;
                    angleY = 0.0f;
                }
                else
                {
                    throw new Exception();
                }

                // プレイヤーの手札を積み上げる
                float motionProgress = 1.0f;
                gameModelBuffer.AddCardOfPlayersPile(player, idOfCard);
                var  movement = new Movement(
                    beginPosition: goCard.transform.position,
                    endPosition: new Vector3(gameViewModel.pileCardsX[player], gameViewModel.pileCardsY[player], gameViewModel.pileCardsZ[player]),
                    beginRotation: goCard.transform.rotation,
                    endRotation: Quaternion.Euler(0, angleY, 180.0f),
                    gameObject: goCard);
                movement.GameObject.transform.position = Vector3.Lerp(movement.BeginPosition, movement.EndPosition, motionProgress);
                movement.GameObject.transform.rotation = Quaternion.Lerp(movement.BeginRotation, movement.EndRotation, motionProgress);

                // 更新
                gameViewModel.pileCardsY[player] += 0.2f;
            }
        }
    }
}
