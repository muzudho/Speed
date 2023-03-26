namespace Assets.Scripts.ThinkingEngine.Models.Game
{
    using System.Collections.Generic;
    using ModelOfThinkingEngine = Assets.Scripts.ThinkingEngine.Models;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.GameBuffer;

    /// <summary>
    /// ゲーム・モデル
    /// 
    /// - プレイヤー別
    /// </summary>
    internal class Player
    {
        // - その他

        internal Player(
            ModelOfGameBuffer.Model gameModelBuffer,
            ModelOfThinkingEngine.Player playerObj)
        {
            this.gameModelBuffer = gameModelBuffer;
            this.playerObj = playerObj;
        }

        // - フィールド

        readonly ModelOfThinkingEngine.Player playerObj;

        readonly ModelOfGameBuffer.Model gameModelBuffer;

        // - メソッド

        /// <summary>
        /// ｎプレイヤーが選択している場札は、先頭から何枚目
        /// 
        /// - 選択中の場札が無いなら、-1
        /// </summary>
        internal HandCardIndex GetIndexOfFocusedCardOfPlayer()
        {
            return this.gameModelBuffer.GetPlayer(this.playerObj).IndexOfFocusedCardOfPlayersObj;
        }

        /// <summary>
        /// ｎプレイヤーの、場札の枚数
        /// </summary>
        /// <returns></returns>
        internal int GetLengthOfPlayerHandCards()
        {
            return this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfPlayersHand.Count;
        }

        /// <summary>
        /// ｎプレイヤーの、場札をリストで取得
        /// </summary>
        /// <returns></returns>
        internal List<IdOfPlayingCards> GetCardsOfPlayerHand()
        {
            return this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfPlayersHand;
        }

        /// <summary>
        /// ｎプレイヤーの、ｍ枚目の場札を取得
        /// </summary>
        /// <param name="handIndexObj"></param>
        /// <returns></returns>
        internal IdOfPlayingCards GetCardAtOfPlayerHand(HandCardIndex handIndexObj)
        {
            return this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfPlayersHand[handIndexObj.AsInt];
        }
    }
}
