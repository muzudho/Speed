namespace Assets.Scripts.Models.Timeline.Spans
{
    /// <summary>
    /// ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
    /// </summary>
    internal class MoveCardToCenterStackFromHandModel
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="player"></param>
        /// <param name="place"></param>
        internal MoveCardToCenterStackFromHandModel(int player, int place)
        {
            this.Player = player;
            this.Place = place;
        }

        // - プロパティ

        internal int Player { get; private set; }
        internal int Place { get; private set; }
    }
}
