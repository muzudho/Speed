namespace Assets.Scripts.ThinkingEngine.Models.Commands
{
    /// <summary>
    /// 右（または左）側の台札１枚を、手札へ移動する
    /// </summary>
    internal class MoveCardsToPileFromCenterStacks : IModel
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="placeObj"></param>
        internal MoveCardsToPileFromCenterStacks(CenterStackPlace placeObj)
        {
            this.PlaceObj = placeObj;
        }

        // - プロパティ

        internal CenterStackPlace PlaceObj { get; private set; }
    }
}
