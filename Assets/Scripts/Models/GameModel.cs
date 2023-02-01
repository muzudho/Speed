namespace Assets.Scripts.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// ゲーム・モデル
    /// 
    /// - 読み取り専用。(Immutable)
    /// </summary>
    class GameModel
    {
        GameModelBuffer gameModelBuffer;

        public GameModel(GameModelBuffer gameModel)
        {
            this.gameModelBuffer = gameModel;
        }

        /// <summary>
        /// 右（または左）の天辺の台札
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        /// <returns></returns>
        internal IdOfPlayingCards GetLastCardOfCenterStack(int place)
        {
            var length = this.GetLengthOfCenterStackCards(place);
            var startIndex = length - 1;
            return this.gameModelBuffer.IdOfCardsOfCenterStacks[place][startIndex]; // 最後のカード
        }

        /// <summary>
        /// ｎプレイヤーが選択している場札は、先頭から何枚目
        /// 
        /// - 選択中の場札が無いなら、-1
        /// </summary>
        /// <param name="player">プレイヤー</param>
        internal int GetIndexOfFocusedCardOfPlayer(int player)
        {
            return this.gameModelBuffer.IndexOfFocusedCardOfPlayers[player];
        }

        /// <summary>
        /// 右（または左）の台札の枚数
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        internal int GetLengthOfCenterStackCards(int place)
        {
            return this.gameModelBuffer.IdOfCardsOfCenterStacks[place].Count;
        }

        /// <summary>
        /// ｎプレイヤーの、場札の枚数
        /// </summary>
        /// <param name="player">プレイヤー</param>
        /// <returns></returns>
        internal int GetLengthOfPlayerHandCards(int player)
        {
            return this.gameModelBuffer.IdOfCardsOfPlayersHand[player].Count;
        }

        /// <summary>
        /// ｎプレイヤーの、場札をリストで取得
        /// </summary>
        /// <param name="player">プレイヤー</param>
        /// <returns></returns>
        internal List<IdOfPlayingCards> GetCardsOfPlayerHand(int player)
        {
            return this.gameModelBuffer.IdOfCardsOfPlayersHand[player];
        }

        /// <summary>
        /// ｎプレイヤーの、ｍ枚目の場札を取得
        /// </summary>
        /// <param name="player"></param>
        /// <param name="handIndex"></param>
        /// <returns></returns>
        internal IdOfPlayingCards GetCardAtOfPlayerHand(int player, int handIndex)
        {
            return this.gameModelBuffer.IdOfCardsOfPlayersHand[player][handIndex];
        }
    }
}
