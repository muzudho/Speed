namespace Assets.Scripts.ThinkingEngine.Models.Game
{
    using System.Collections.Generic;

    /// <summary>
    /// プレイヤーｎのゲーム・モデル
    /// </summary>
    partial class Default
    {
        // - メソッド

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
