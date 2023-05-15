namespace Assets.Scripts.Vision.Models
{
    using Assets.Scripts.ThinkingEngine.Models;

    /// <summary>
    /// フォーカスが当たっている場札のカード（のインデックス）
    /// </summary>
    internal class FocusedHandCard
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="isPickup"></param>
        /// <param name="index"></param>
        internal FocusedHandCard(bool isPickup, HandCardIndex index)
        {
            this.IsPickUp = isPickup;
            this.Index = index;
        }

        // - 静的フィールド

        internal static readonly FocusedHandCard Empty = new FocusedHandCard(false, HandCardIndex.Empty);

        /// <summary>
        /// 先頭の場札をピックアップ
        /// </summary>
        internal static readonly FocusedHandCard PickupFirst = new FocusedHandCard(true, HandCardIndex.First);

        // - プロパティ

        /// <summary>
        /// ピックアップしているか？
        /// </summary>
        internal bool IsPickUp { get; private set; }

        /// <summary>
        /// プレイヤーが選択している場札は、先頭から何枚目
        /// 
        /// - 選択中の場札が無いなら、-1
        /// </summary>
        internal HandCardIndex Index { get; private set; }
    }
}
