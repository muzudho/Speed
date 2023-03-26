﻿namespace Assets.Scripts.ThinkingEngine.Models
{
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;

    static class LegalMove
    {
        // - メソッド

        internal static bool CanPutToCenterStack(
            ModelOfGame.Default gameModel,
            Player playerObj,
            HandCardIndex indexObj,
            CenterStackPlace placeOfCenterStackObj)
        {
            if (indexObj == Commons.HandCardIndexNoSelected)
            {
                return false;
            }

            IdOfPlayingCards topCard = gameModel.GetLastCardOfCenterStack(placeOfCenterStackObj);
            if (topCard == IdOfPlayingCards.None)
            {
                return false;
            }

            var numberOfPickup = gameModel.GetCardsOfPlayerHand(playerObj)[indexObj.AsInt];
            var numberOfTopCard = topCard;

            return CardNumberHelper.IsNext(
                topCard: numberOfTopCard,
                pickupCard: numberOfPickup);
        }
    }
}
