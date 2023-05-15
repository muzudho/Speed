namespace Assets.Scripts.Vision.Models
{
    using Assets.Scripts.ThinkingEngine.Models;

    /// <summary>
    /// フォーカスが当たっている場札のカード（のインデックス）
    /// </summary>
    internal class FocusedCard
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="isPickup"></param>
        /// <param name="hand"></param>
        internal FocusedCard(bool isPickup, HandCardIndex hand)
        {
            this.IsPickUp = isPickup;
            this.Hand = hand;
        }

        // - プロパティ

        /// <summary>
        /// ピックアップしているか？
        /// </summary>
        internal bool IsPickUp { get; private set; }

        /// <summary>
        /// 場札のインデックス
        /// </summary>
        internal HandCardIndex Hand { get; private set; }
    }
}
