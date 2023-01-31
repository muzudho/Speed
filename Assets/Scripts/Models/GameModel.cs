namespace Assets.Scripts.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// ゲーム・モデル
    /// 
    /// - Immutable
    /// </summary>
    class GameModel
    {
        GameModelBuffer gameModelBuffer;

        public GameModel(GameModelBuffer gameModel)
        {
            this.gameModelBuffer = gameModel;
        }

        internal IdOfPlayingCards GetCardOfCenterStack(int place, int startIndex)
        {
            return this.gameModelBuffer.IdOfCardsOfCenterStacks[place][startIndex];
        }

        /// <summary>
        /// 天辺の台札
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        internal IdOfPlayingCards GetLastCardOfCenterStack(int place)
        {
            var length = this.GetLengthOfCenterStackCards(place);
            return this.GetCardOfCenterStack(place, length - 1); // 最後のカード
        }

        /// <summary>
        /// プレイヤーが選択している場札は、先頭から何枚目
        /// 
        /// - 選択中の場札が無いなら、-1
        /// </summary>
        internal int GetIndexOfFocusedCardOfPlayer(int player)
        {
            return this.gameModelBuffer.IndexOfFocusedCardOfPlayers[player];
        }

        /// <summary>
        /// 台札の枚数
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        internal int GetLengthOfCenterStackCards(int place)
        {
            return this.gameModelBuffer.IdOfCardsOfCenterStacks[place].Count;
        }

        /// <summary>
        /// 場札の枚数
        /// </summary>
        /// <returns></returns>
        internal int GetLengthOfPlayerHandCards(int player)
        {
            return this.gameModelBuffer.IdOfCardsOfPlayersHand[player].Count;
        }

        internal List<IdOfPlayingCards> GetCardsOfPlayerHand(int player)
        {
            return this.gameModelBuffer.IdOfCardsOfPlayersHand[player];
        }

        internal IdOfPlayingCards GetCardAtOfPlayerHand(int player, int handIndex)
        {
            return this.gameModelBuffer.IdOfCardsOfPlayersHand[player][handIndex];
        }
    }
}
