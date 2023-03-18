namespace Assets.Scripts.ThinkingEngine.Models.CommandArgs
{
    /// <summary>
    /// ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
    /// </summary>
    internal class MoveCardToCenterStackFromHandModel : ICommandArg
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="place"></param>
        internal MoveCardToCenterStackFromHandModel(Player playerObj, int place)
        {
            this.PlayerObj = playerObj;
            this.Place = place;
        }

        // - プロパティ

        internal Player PlayerObj { get; private set; }
        internal int Place { get; private set; }
    }
}
