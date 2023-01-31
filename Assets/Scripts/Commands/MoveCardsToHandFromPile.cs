namespace Assets.Scripts.Commands
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;

    static class MoveCardsToHandFromPile
    {
        /// <summary>
        /// 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
        /// 
        /// - 画面上の場札は位置調整される
        /// </summary>
        internal static void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel, int player, int numberOfCards)
        {
            // 手札の上の方からｎ枚抜いて、場札へ移動する
            var length = gameModelBuffer.IdOfCardsOfPlayersPile[player].Count; // 手札の枚数
            if (numberOfCards <= length)
            {
                GameModel gameModel = new GameModel(gameModelBuffer);
                var startIndex = length - numberOfCards;

                gameModelBuffer.MoveCardsToHandFromPile(player, startIndex, numberOfCards);

                gameViewModel.ArrangeHandCards(gameModel, player);
            }
        }
    }
}
