namespace Assets.Scripts.ThinkingEngine.Models.Game.Writer
{
    using System.Collections.Generic;
    using ModelOfThinkingEngine = Assets.Scripts.ThinkingEngine.Models;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;

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
        /// - 編集可
        /// - 選択中の場札が無いなら、-1
        /// </summary>
        internal void UpdateIndexOfFocusedCard(HandCardIndex hand)
        {
            this.gameModelBuffer.GetPlayer(this.playerObj).UpdateIndexOfFocusedCard(hand);
        }

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

        #region メソッド（手札関連）
        /// <summary>
        /// 手札をクリアー
        /// </summary>
        internal void ClearCardsOfPile()
        {
            this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfPile.Clear();
        }

        /// <summary>
        /// 手札を追加
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="idOfCard"></param>
        internal void AddCardOfPile(IdOfPlayingCards idOfCard)
        {
            this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfPile.Add(idOfCard);
        }

        /// <summary>
        /// 手札を削除
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="startIndexObj"></param>
        /// <param name="numberOfCards"></param>
        internal void RemoveRangeCardsOfPile(PlayerPileCardIndex startIndexObj, int numberOfCards)
        {
            this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfPile.RemoveRange(startIndexObj.AsInt, numberOfCards);
        }
        #endregion

        #region メソッド（場札関連）
        /// <summary>
        /// 場札をクリアー
        /// </summary>
        internal void ClearCardsOfHand()
        {
            this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfHand.Clear();
        }

        /// <summary>
        /// 場札を追加
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="idOfCards"></param>
        internal void AddRangeCardsOfHand(List<IdOfPlayingCards> idOfCards)
        {
            this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfHand.AddRange(idOfCards);
        }

        /// <summary>
        /// 場札を削除
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="handIndexObj"></param>
        internal void RemoveCardAtOfHand(HandCardIndex handIndexObj)
        {
            this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfHand.RemoveAt(handIndexObj.AsInt);
        }
        #endregion

        /// <summary>
        /// 手札から場札へ移動
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="startIndexObj"></param>
        /// <param name="numberOfCards"></param>
        internal void MoveCardsToHandFromPile(PlayerPileCardIndex startIndexObj, int numberOfCards)
        {
            var idOfCards = this.gameModelBuffer.GetPlayer(this.playerObj).IdOfCardsOfPile.GetRange(startIndexObj.AsInt, numberOfCards);

            this.RemoveRangeCardsOfPile(startIndexObj, numberOfCards);
            this.AddRangeCardsOfHand(idOfCards);
        }
    }
}
