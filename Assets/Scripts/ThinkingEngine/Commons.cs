namespace Assets.Scripts.ThinkingEngine
{
    using Assets.Scripts.ThinkingEngine.Models;

    /// <summary>
    /// よく使う不変値
    /// </summary>
    static class Commons
    {
        internal static readonly Player Player1 = new Player(0);
        internal static readonly Player Player2 = new Player(1);
        internal static readonly Player[] Players = new Player[]
        {
            Player1,
            Player2,
        };

        /// <summary>
        /// 1プレイヤーから見て右側の台札
        /// </summary>
        internal static readonly CenterStackPlace RightCenterStack = new CenterStackPlace(0);
        internal static readonly CenterStackPlace LeftCenterStack = new CenterStackPlace(1);
        internal static readonly CenterStackPlace[] CenterStacks = new CenterStackPlace[]
        {
            RightCenterStack,
            LeftCenterStack,
        };
    }
}
