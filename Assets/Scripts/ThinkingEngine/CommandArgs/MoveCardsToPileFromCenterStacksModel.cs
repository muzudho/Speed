namespace Assets.Scripts.ThikningEngine.CommandArgs
{
    /// <summary>
    /// 右（または左）側の台札１枚を、手札へ移動する
    /// </summary>
    internal class MoveCardsToPileFromCenterStacksModel : ICommandArgs
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="place"></param>
        internal MoveCardsToPileFromCenterStacksModel(int place)
        {
            this.Place = place;
        }

        // - プロパティ

        internal int Place { get; private set; }
    }
}
