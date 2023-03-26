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
        /// <param name="playerObj">プレイヤー</param>
        internal HandCardIndex GetIndexOfFocusedCardOfPlayer(Player playerObj)
        {
            return this.gameModelBuffer.IndexOfFocusedCardOfPlayersObj[playerObj.AsInt];
        }

        /// <summary>
        /// ｎプレイヤーの、場札の枚数
        /// </summary>
        /// <param name="playerObj">プレイヤー</param>
        /// <returns></returns>
        internal int GetLengthOfPlayerHandCards(Player playerObj)
        {
            return this.gameModelBuffer.Players[playerObj.AsInt].IdOfCardsOfPlayersHand.Count;
        }

        /// <summary>
        /// ｎプレイヤーの、場札をリストで取得
        /// </summary>
        /// <param name="playerObj">プレイヤー</param>
        /// <returns></returns>
        internal List<IdOfPlayingCards> GetCardsOfPlayerHand(Player playerObj)
        {
            return this.gameModelBuffer.Players[playerObj.AsInt].IdOfCardsOfPlayersHand;
        }

        /// <summary>
        /// ｎプレイヤーの、ｍ枚目の場札を取得
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="handIndexObj"></param>
        /// <returns></returns>
        internal IdOfPlayingCards GetCardAtOfPlayerHand(Player playerObj, HandCardIndex handIndexObj)
        {
            return this.gameModelBuffer.Players[playerObj.AsInt].IdOfCardsOfPlayersHand[handIndexObj.AsInt];
        }
    }
}
