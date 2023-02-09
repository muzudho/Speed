namespace Assets.Scripts.ThinkingEngine.Model
{
    internal class LegalMove
    {
        // - メソッド

        internal static bool CanPutToCenterStack(GameModel gameModel, int player, int place)
        {
            int index = gameModel.GetIndexOfFocusedCardOfPlayer(player);
            if (index == -1)
            {
                return false;
            }

            IdOfPlayingCards topCard = gameModel.GetLastCardOfCenterStack(place);
            if (topCard == IdOfPlayingCards.None)
            {
                return false;
            }

            var numberOfPickup = gameModel.GetCardsOfPlayerHand(player)[index].Number();
            int numberOfTopCard = topCard.Number();

            // とりあえず差分を取る。
            // 負数が出ると、負数の剰余はプログラムによって結果が異なるので、面倒だ。
            // 割る数を先に足しておけば、剰余をしても負数にはならない
            int divisor = 13; // 法
            int remainder = (numberOfTopCard - numberOfPickup + divisor) % divisor;

            return remainder == 1 || remainder == divisor - 1;
        }
    }
}
