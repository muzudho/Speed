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

        readonly ModelOfGameBuffer.Model gameModelBuffer;

        readonly ModelOfThinkingEngine.Player playerObj;

        // - メソッド

        /// <summary>
        /// ｎプレイヤーが選択している場札は、先頭から何枚目
        /// 
        /// - 選択中の場札が無いなら、-1
        /// </summary>
        internal HandCardIndex GetIndexOfFocusedCard()
        {
            return this.gameModelBuffer.GetPlayer(this.playerObj).IndexOfFocusedCard;
        }

        /// <summary>
        /// ｎプレイヤーの、場札の枚数
        /// </summary>
        /// <returns></returns>
        internal int GetLengthOfHandCards()
        {
            return this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfHand.Count;
        }

        /// <summary>
        /// ｎプレイヤーの、場札をリストで取得
        /// </summary>
        /// <returns></returns>
        internal List<IdOfPlayingCards> GetCardsOfHand()
        {
            return this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfHand;
        }

        /// <summary>
        /// ｎプレイヤーの、ｍ枚目の場札を取得
        /// </summary>
        /// <param name="handIndexObj"></param>
        /// <returns></returns>
        internal IdOfPlayingCards GetCardAtOfHand(HandCardIndex handIndexObj)
        {
            return this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfHand[handIndexObj.AsInt];
        }
    }
}
