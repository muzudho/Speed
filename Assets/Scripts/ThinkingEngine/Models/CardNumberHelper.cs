namespace Assets.Scripts.ThinkingEngine.Models
{
    internal class CardNumberHelper
    {
        // - プロパティ

        /// <summary>
        /// 法
        /// 
        /// - カードのエースからキングの枚数
        /// </summary>
        internal static int Divisor => 13;

        // - メソッド

        /// <summary>
        /// 余りを取る（差分）
        /// 
        /// - カードの A と K はつながっているものとする
        /// </summary>
        /// <param name="topCard"></param>
        /// <param name="pickupCard"></param>
        /// <returns></returns>
        internal static int GetRemainder(
            IdOfPlayingCards topCard,
            IdOfPlayingCards pickupCard)
        {
            // 負数が出ると、負数の剰余はプログラムによって結果が異なるので、面倒だ。
            // 割る数を先に足しておけば、剰余をしても負数にはならない
            int divisor = 13; // 法
            int remainder = (topCard.Number() - pickupCard.Number() + divisor) % divisor;

            return remainder;
        }

        /// <summary>
        /// 隣か
        /// </summary>
        /// <returns></returns>
        internal static bool IsNext(
            IdOfPlayingCards topCard,
            IdOfPlayingCards pickupCard)
        {
            // とりあえず余りを求める
            var remainder = CardNumberHelper.GetRemainder(
                topCard: topCard,
                pickupCard: pickupCard);

            return remainder == 1 || remainder == CardNumberHelper.Divisor - 1;
        }
    }
}
