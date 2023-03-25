namespace Assets.Scripts.Vision.Models.Input
{
    /// <summary>
    /// プレイヤーの入力
    /// </summary>
    internal class Player
    {
        /// <summary>
        /// 入力の権利
        /// </summary>
        internal Rights Rights { get; private set; } = new Rights();
    }
}
