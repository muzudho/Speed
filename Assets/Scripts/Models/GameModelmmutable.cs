using System.Collections.Generic;

namespace Assets.Scripts.Models
{
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
            return this.gameModelBuffer.goCenterStacksCards[place][startIndex];
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
        /// 手札
        /// 0: １プレイヤー（黒色）
        /// 1: ２プレイヤー（黒色）
        /// </summary>
        internal IdOfPlayingCards GetPlayersPileCard(int player, int indexOfCard)
        {
            return this.gameModelBuffer.goPlayersPileCards[player][indexOfCard];
        }

        /// <summary>
        /// 場札（プレイヤー側でオープンしている札）
        /// 0: １プレイヤー（黒色）
        /// 1: ２プレイヤー（黒色）
        /// </summary>
        internal IdOfPlayingCards GetPlayersHandCard(int player, int indexOfCard)
        {
            return this.gameModelBuffer.goPlayersHandCards[player][indexOfCard];
        }

        /// <summary>
        /// 台札（画面中央に積んでいる札）
        /// 0: 右
        /// 1: 左
        /// </summary>
        internal IdOfPlayingCards GetCenterStacksCards(int player, int indexOfCard)
        {
            return this.gameModelBuffer.goCenterStacksCards[player][indexOfCard];
        }

        /// <summary>
        /// 台札の枚数
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        internal int GetLengthOfCenterStackCards(int place)
        {
            return this.gameModelBuffer.goCenterStacksCards[place].Count;
        }

        /// <summary>
        /// 手札の枚数
        /// </summary>
        /// <returns></returns>
        internal int GetLengthOfPlayerPileCards(int player)
        {
            return this.gameModelBuffer.goPlayersPileCards[player].Count;
        }

        internal List<IdOfPlayingCards> GetRangeCardsOfPlayerPile(int player, int startIndex, int numberOfCards)
        {
            return this.gameModelBuffer.goPlayersPileCards[player].GetRange(startIndex, numberOfCards);
        }

        /// <summary>
        /// 場札の枚数
        /// </summary>
        /// <returns></returns>
        internal int GetLengthOfPlayerHandCards(int player)
        {
            return this.gameModelBuffer.goPlayersHandCards[player].Count;
        }

        internal List<IdOfPlayingCards> GetCardsOfPlayerHand(int player)
        {
            return this.gameModelBuffer.goPlayersHandCards[player];
        }

        internal IdOfPlayingCards GetCardAtOfPlayerHand(int player, int handIndex)
        {
            return this.gameModelBuffer.goPlayersHandCards[player][handIndex];
        }
    }
}
