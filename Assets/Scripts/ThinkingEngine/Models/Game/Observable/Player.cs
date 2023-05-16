namespace Assets.Scripts.ThinkingEngine.Models.Game.Observable
{
    using System.Collections.Generic;
    using ModelOfThinkingEngine = Assets.Scripts.ThinkingEngine.Models;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;
    using Assets.Scripts.Vision.Models;

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
        /// プレイヤーが選択している場札
        /// </summary>
        internal FocusedHandCard GetFocusedHandCardObj()
        {
            return this.gameModelBuffer.GetPlayer(this.playerObj).FocusedHandCardObj;
        }

        /// <summary>
        /// プレイヤーの、場札の枚数
        /// </summary>
        /// <returns></returns>
        internal int GetLengthOfHandCards()
        {
            return this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfHand.Count;
        }

        /// <summary>
        /// プレイヤーの、場札をリストで取得
        /// </summary>
        /// <returns></returns>
        internal List<IdOfPlayingCards> GetCardsOfHand()
        {
            return this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfHand;
        }

        /// <summary>
        /// プレイヤーの、手札をリストで取得
        /// </summary>
        /// <returns></returns>
        internal List<IdOfPlayingCards> GetCardsOfPile()
        {
            return this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfPile;
        }

        /// <summary>
        /// プレイヤーの、ｍ枚目の場札を取得
        /// </summary>
        /// <param name="handIndexObj"></param>
        /// <returns></returns>
        internal IdOfPlayingCards GetCardAtOfHand(HandCardIndex handIndexObj)
        {
            return this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfHand[handIndexObj.AsInt];
        }
    }
}
