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
            List<IdOfPlayingCards> idOfCardsOfCenterStacks,
            List<IdOfPlayingCards> idOfCardsOfPlayersPile,
            List<IdOfPlayingCards> idOfCardsOfPlayersHand)
        {
            this.IdOfCardsOfCenterStacks = idOfCardsOfCenterStacks;
            this.IdOfCardsOfPlayersPile = idOfCardsOfPlayersPile;
            this.IdOfCardsOfPlayersHand = idOfCardsOfPlayersHand;
        }

        // - プロパティ

        /// <summary>
        /// 台札
        /// 
        /// - 画面中央に積んでいる札
        /// - 0: 右
        /// - 1: 左
        /// </summary>
        internal List<IdOfPlayingCards> IdOfCardsOfCenterStacks { get; private set; }

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
