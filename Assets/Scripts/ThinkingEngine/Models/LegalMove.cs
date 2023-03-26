namespace Assets.Scripts.ThinkingEngine.Models
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

            IdOfPlayingCards topCard = gameModel.GetCenterStack(placeOfCenterStackObj).GetLastCard();
            if (topCard == IdOfPlayingCards.None)
            {
                return false;
            }

            var numberOfPickup = gameModel.GetPlayer(playerObj).GetCardsOfHand()[indexObj.AsInt];
            var numberOfTopCard = topCard;

            return CardNumberHelper.IsNext(
                topCard: numberOfTopCard,
                pickupCard: numberOfPickup);
        }
    }
}
