namespace Assets.Scripts.ThinkingEngine.Models.Game.Buffer
{
    using System.Collections.Generic;

    /// <summary>
    /// ゲーム・モデル・バッファー
    /// 
    /// - 編集可
    /// - プレイヤー別
    /// </summary>
    internal class Player
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        internal Player(
            List<IdOfPlayingCards> idOfCardsOfPile,
            List<IdOfPlayingCards> idOfCardsOfHand,
            HandCardIndex indexOfFocusedCard)
        {
            this.IdOfCardsOfPile = idOfCardsOfPile;
            this.IdOfCardsOfHand = idOfCardsOfHand;
            this.IndexOfFocusedCard = indexOfFocusedCard;
        }

        // - プロパティ

        /// <summary>
        /// 手札
        /// 
        /// - プレイヤー側で積んでる札
        /// </summary>
        internal List<IdOfPlayingCards> IdOfCardsOfPile { get; private set; }

        /// <summary>
        /// 場札
        /// 
        /// - プレイヤー側でオープンしている札
        /// - 0: １プレイヤー（黒色）
        /// - 1: ２プレイヤー（黒色）
        /// </summary>
        internal List<IdOfPlayingCards> IdOfCardsOfHand { get; private set; }

        /// <summary>
        /// ｎプレイヤーが選択している場札は、先頭から何枚目
        /// 
        /// - 編集可
        /// - 選択中の場札が無いなら、-1
        /// </summary>
        internal HandCardIndex IndexOfFocusedCard { get; set; }

        // - メソッド

        #region メソッド（手札関連）
        /// <summary>
        /// 手札を追加
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="idOfCard"></param>
        internal void AddCardOfPile(IdOfPlayingCards idOfCard)
        {
            this.IdOfCardsOfPile.Add(idOfCard);
        }

        /// <summary>
        /// 手札を削除
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="startIndexObj"></param>
        /// <param name="numberOfCards"></param>
        internal void RemoveRangeCardsOfPile(PlayerPileCardIndex startIndexObj, int numberOfCards)
        {
            this.IdOfCardsOfPile.RemoveRange(startIndexObj.AsInt, numberOfCards);
        }
        #endregion

        #region メソッド（場札関連）
        /// <summary>
        /// 場札を追加
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="idOfCards"></param>
        internal void AddRangeCardsOfHand(List<IdOfPlayingCards> idOfCards)
        {
            this.IdOfCardsOfHand.AddRange(idOfCards);
        }

        /// <summary>
        /// 場札を削除
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="handIndexObj"></param>
        internal void RemoveCardAtOfHand(HandCardIndex handIndexObj)
        {
            this.IdOfCardsOfHand.RemoveAt(handIndexObj.AsInt);
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
            var idOfCards = this.IdOfCardsOfPile.GetRange(startIndexObj.AsInt, numberOfCards);

            this.RemoveRangeCardsOfPile(startIndexObj, numberOfCards);
            this.AddRangeCardsOfHand(idOfCards);
        }
    }
}
