namespace Assets.Scripts.ThinkingEngine
{
    using Assets.Scripts.ThinkingEngine.Models;

    static class Commons
    {
        internal static readonly Player Player1 = new Player(0);
        internal static readonly Player Player2 = new Player(1);
        internal static readonly Player[] Players = new Player[]
        {
            Player1,
            Player2,
        };
    }
}
