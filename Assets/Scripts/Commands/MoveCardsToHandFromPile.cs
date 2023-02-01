namespace Assets.Scripts.Commands
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;

    class MoveCardsToHandFromPile : ICommand
    {
        // - その他（生成）

        internal MoveCardsToHandFromPile(int player, int numberOfCards)
        {
            Player = player;
            NumberOfCards = numberOfCards;
        }

        // - プロパティ

        int Player { get; set; }
        int NumberOfCards { get; set; }

        // - メソッド

        /// <summary>
        /// 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
        /// 
        /// - 画面上の場札は位置調整される
        /// </summary>
        public void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            // 手札の上の方からｎ枚抜いて、場札へ移動する
            var length = gameModelBuffer.IdOfCardsOfPlayersPile[Player].Count; // 手札の枚数
            if (NumberOfCards <= length)
            {
                GameModel gameModel = new GameModel(gameModelBuffer);
                var startIndex = length - NumberOfCards;

                gameModelBuffer.MoveCardsToHandFromPile(Player, startIndex, NumberOfCards);

                gameViewModel.ArrangeHandCards(gameModel, Player);
            }
        }
    }
}
