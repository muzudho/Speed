namespace Assets.Scripts.ThinkingEngine.Models
{
    using ModelOfObservableGame = Assets.Scripts.ThinkingEngine.Models.Game.Observable;

    static class LegalMove
    {
        // - メソッド

        /// <summary>
        /// 台札へ、カードを置いていいか？
        /// </summary>
        /// <param name="observableGameModel">ゲーム・モデル</param>
        /// <param name="playerObj">プレイヤー</param>
        /// <param name="indexObj">手札のインデックス</param>
        /// <param name="placeOfCenterStackObj">どちらの台札か</param>
        /// <returns></returns>
        internal static bool CanPutCardToCenterStack(
            ModelOfObservableGame.Model observableGameModel,
            Player playerObj,
            HandCardIndex indexObj,
            CenterStackPlace placeOfCenterStackObj)
        {
            // 場札が選ばれていない
            if (indexObj == Commons.HandCardIndexNoSelected)
            {
                return false;
            }

            // 台札の天辺の札はある
            IdOfPlayingCards topCard = observableGameModel.GetCenterStack(placeOfCenterStackObj).GetLastCard();
            if (topCard == IdOfPlayingCards.None)
            {
                return false;
            }

            // 範囲外か？
            if(observableGameModel.GetPlayer(playerObj).GetLengthOfHandCards() <= indexObj.AsInt)
            {
                return false;
            }

            // 選んでいる場札
            var pickupHand = observableGameModel.GetPlayer(playerObj).GetCardsOfHand()[indexObj.AsInt];

            // 隣の番号か？
            return CardNumberHelper.IsNextNumber(
                topCard: topCard,
                pickupCard: pickupHand);
        }
    }
}
