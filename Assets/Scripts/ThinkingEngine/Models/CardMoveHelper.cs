namespace Assets.Scripts.ThinkingEngine.Models
{
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
            int indexOnCenterStackToNextCard,
            ModelOfGame gameModel,
            CenterStackPlace placeObj,
            IdOfPlayingCards targetToRemoveObj,
            out IdOfPlayingCards previousCard)
        {
            // 台札はあるか？
            if (0 < indexOnCenterStackToNextCard)
            {
                // 下のカード
                previousCard = gameModel.GetCenterStack(placeObj).GetCard(indexOnCenterStackToNextCard);
                // Debug.Log($"テストC topCard:{previousCard.Number()} pickupCard:{targetToRemoveObj.Number()}");

                // 連続する数か？
                if (CardNumberHelper.IsNext(
                    topCard: previousCard,
                    pickupCard: targetToRemoveObj))
                {
                    return false;
                }

                return true;
            }

            previousCard = IdOfPlayingCards.None;
            return false;
        }
    }
}
