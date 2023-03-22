namespace Assets.Scripts.ThinkingEngine.Models.CommandParameters
{
    /// <summary>
    /// ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
    /// </summary>
    internal class MoveCardToCenterStackFromHand : IModel
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="placeObj"></param>
        internal MoveCardToCenterStackFromHand(Player playerObj, CenterStackPlace placeObj)
        {
            this.PlayerObj = playerObj;
            this.PlaceObj = placeObj;
        }

        // - プロパティ

        internal Player PlayerObj { get; private set; }
        internal CenterStackPlace PlaceObj { get; private set; }
    }
}
