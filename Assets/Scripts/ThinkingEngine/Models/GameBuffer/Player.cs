namespace Assets.Scripts.ThinkingEngine.Models.GameBuffer
{
    using Assets.Scripts.Vision.Models;
    using System.Collections.Generic;
    using ModelOfThinkingEngine = Assets.Scripts.ThinkingEngine.Models;

    /// <summary>
    /// ゲーム・モデル・バッファー
    /// 
    /// - プレイヤー別
    /// </summary>
    internal class Player
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        internal Player(
            List<IdOfPlayingCards> idOfCardsOfPlayersPile,
            List<IdOfPlayingCards> idOfCardsOfPlayersHand)
        {
            this.IdOfCardsOfPlayersPile = idOfCardsOfPlayersPile;
            this.IdOfCardsOfPlayersHand = idOfCardsOfPlayersHand;
        }

        // - プロパティ

        /// <summary>
        /// 手札
        /// 
        /// - プレイヤー側で積んでる札
        /// </summary>
        internal List<IdOfPlayingCards> IdOfCardsOfPlayersPile { get; private set; }

        /// <summary>
        /// 場札
        /// 
        /// - プレイヤー側でオープンしている札
        /// - 0: １プレイヤー（黒色）
        /// - 1: ２プレイヤー（黒色）
        /// </summary>
        internal List<IdOfPlayingCards> IdOfCardsOfPlayersHand { get; private set; }

        // - メソッド

        #region メソッド（手札関連）
        /// <summary>
        /// 手札を追加
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="idOfCard"></param>
        internal void AddCardOfPlayersPile(IdOfPlayingCards idOfCard)
        {
            this.IdOfCardsOfPlayersPile.Add(idOfCard);
        }

        /// <summary>
        /// 手札を削除
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="startIndexObj"></param>
        /// <param name="numberOfCards"></param>
        internal void RemoveRangeCardsOfPlayerPile(PlayerPileCardIndex startIndexObj, int numberOfCards)
        {
            this.IdOfCardsOfPlayersPile.RemoveRange(startIndexObj.AsInt, numberOfCards);
        }
        #endregion

        #region メソッド（場札関連）
        /// <summary>
        /// 場札を追加
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="idOfCards"></param>
        internal void AddRangeCardsOfPlayerHand(List<IdOfPlayingCards> idOfCards)
        {
            this.IdOfCardsOfPlayersHand.AddRange(idOfCards);
        }

        /// <summary>
        /// 場札を削除
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="handIndexObj"></param>
        internal void RemoveCardAtOfPlayerHand(HandCardIndex handIndexObj)
        {
            this.IdOfCardsOfPlayersHand.RemoveAt(handIndexObj.AsInt);
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
            var idOfCards = this.IdOfCardsOfPlayersPile.GetRange(startIndexObj.AsInt, numberOfCards);

            this.RemoveRangeCardsOfPlayerPile(startIndexObj, numberOfCards);
            this.AddRangeCardsOfPlayerHand(idOfCards);
        }
    }
}
