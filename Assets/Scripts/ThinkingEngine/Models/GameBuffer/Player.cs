namespace Assets.Scripts.ThinkingEngine.Models.GameBuffer
{
    using System.Collections.Generic;

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
    }
}
