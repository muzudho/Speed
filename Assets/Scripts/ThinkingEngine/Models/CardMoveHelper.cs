namespace Assets.Scripts.ThinkingEngine.Models
{
    using UnityEngine;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game.Model;

    /// <summary>
    /// カード移動ヘルパー
    /// </summary>
    internal static class CardMoveHelper
    {
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
            if (0 < indexOnCenterStackToNextCard)
            {
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

            previousCard = IdOfPlayingCards.None;
            return false;
        }
    }
}
