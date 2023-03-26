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
            List<IdOfPlayingCards> idOfCardsOfCenterStacks)
        {
            this.IdOfCardsOfCenterStacks = idOfCardsOfCenterStacks;
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
    }
}
