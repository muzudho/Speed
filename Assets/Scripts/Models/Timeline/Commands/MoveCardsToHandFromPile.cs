namespace Assets.Scripts.Models.Timeline.Commands
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;
    using UnityEngine.SocialPlatforms.Impl;

    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    class MoveCardsToHandFromPile : AbstractCommand
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="player">ｎプレイヤー</param>
        /// <param name="numberOfCards">カードがｍ枚</param>
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
        public override void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            // 手札の上の方からｎ枚抜いて、場札へ移動する
            var length = gameModelBuffer.IdOfCardsOfPlayersPile[Player].Count; // 手札の枚数
            if (NumberOfCards <= length)
            {
                // もし、場札が空っぽのところへ、手札を配ったのなら、先頭の場札をピックアップする
                if (gameModelBuffer.IndexOfFocusedCardOfPlayers[Player] == -1)
                {
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[Player] = 0;
                }

                GameModel gameModel = new GameModel(gameModelBuffer);
                var startIndex = length - NumberOfCards;

                gameModelBuffer.MoveCardsToHandFromPile(Player, startIndex, NumberOfCards);

                gameViewModel.ArrangeHandCards(gameModel, Player);
            }
        }
    }
}
