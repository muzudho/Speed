namespace Assets.Scripts.ThinkingEngine.Models
{
    using Assets.Scripts.ThinkingEngine;
    using System;
    using UnityEngine;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game.Model;
    using ModelOfThinkingEngine = Assets.Scripts.ThinkingEngine.Models;

    /// <summary>
    /// カード移動ヘルパー
    /// </summary>
    internal static class CardMoveHelper
    {
        /// <summary>
        /// すべてのカードを、色分けして、黒色なら１プレイヤーの、赤色なら２プレイヤーの、手札に乗せる
        /// </summary>
        /// <param name="idOfCard"></param>
        /// <returns></returns>
        internal static ModelOfThinkingEngine.Player GetPlayerAtStart(IdOfPlayingCards idOfCard)
        {
            ModelOfThinkingEngine.Player playerObj;
            switch (idOfCard.Suit())
            {
                case ModelOfThinkingEngine.IdOfCardSuits.Clubs:
                case ModelOfThinkingEngine.IdOfCardSuits.Spades:
                    playerObj = Commons.Player1;
                    break;

                case ModelOfThinkingEngine.IdOfCardSuits.Diamonds:
                case ModelOfThinkingEngine.IdOfCardSuits.Hearts:
                    playerObj = Commons.Player2;
                    break;

                default:
                    throw new Exception();
            }

            return playerObj;
        }

        /// <summary>
        /// TODO 場札を台札へ移動する要求を出したが、相手の方が早く場札を台札へ移動するのを先着し、
        /// 自分の場札をまた場へ戻すケースか？
        /// </summary>
        /// <param name="indexOnCenterStackToNextCard">これから置く札の台札でのインデックス</param>
        /// <returns></returns>
        internal static bool IsBoomerang(
            ModelOfGame gameModel,
            Player playerObj,
            CenterStackPlace placeObj,
            out IdOfPlayingCards previousCard)
        {
            // これから置く札の台札でのインデックス
            var indexOnCenterStackToNextCard = gameModel.GetCenterStack(placeObj).GetLength();

            // 何枚目の場札をピックアップしているか
            var indexToRemoveObj = gameModel.GetPlayer(playerObj).GetIndexOfFocusedCard();

            // 範囲外は無視
            if (indexToRemoveObj < Commons.HandCardIndexFirst || gameModel.GetPlayer(playerObj).GetCardsOfHand().Count <= indexToRemoveObj.AsInt)
            {
                // Ignored
                previousCard = IdOfPlayingCards.None;
                return false;
            }

            // 確定：場札から台札へ移動するカード
            var targetToRemoveObj = gameModel.GetPlayer(playerObj).GetCardAtOfHand(indexToRemoveObj);

            // 台札はあるか？
            if (indexOnCenterStackToNextCard == 0)
            {
                // 前のカードはないし、ブーメランでもない
                previousCard = IdOfPlayingCards.None;
                return false;
            }

            // 下のカード
            Debug.Log($"下のカードテストC indexOnCenterStackToNextCard:{indexOnCenterStackToNextCard} 台札サイズ:{gameModel.GetCenterStack(placeObj).GetLength()}");
            previousCard = gameModel.GetCenterStack(placeObj).GetCard(indexOnCenterStackToNextCard - 1);
            // Debug.Log($"テストC topCard:{previousCard.Number()} pickupCard:{targetToRemoveObj.Number()}");

            // 連続する数か？
            if (CardNumberHelper.IsNext(
                topCard: previousCard,
                pickupCard: targetToRemoveObj))
            {
                return true;
            }

            return false;
        }
    }
}
