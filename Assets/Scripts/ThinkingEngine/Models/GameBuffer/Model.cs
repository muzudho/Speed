namespace Assets.Scripts.ThinkingEngine.Models.GameBuffer
{
    using Assets.Scripts.Vision.Models;
    using ModelOfThinkingEngine = Assets.Scripts.ThinkingEngine.Models;

    /// <summary>
    /// ゲームの状態
    /// 
    /// - 編集可能
    /// </summary>
    public class Model
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        internal Model(
            CenterStack[] centerStacks,
            Player[] players)
        {
            this.CenterStacks = centerStacks;
            this.Players = players;
        }

        // - プロパティ

        /// <summary>
        /// 対局中か？
        /// </summary>
        public bool IsGameActive { get; set; }

        // ゲーム内経過時間
        internal GameSeconds ElapsedTimeObj { get; set; } = GameSeconds.Zero;

        #region プロパティ（台札別）
        /// <summary>
        /// ゲーム・モデル・バッファー
        /// 
        /// - 台札別
        /// </summary>
        CenterStack[] CenterStacks { get; set; }

        internal CenterStack GetCenterStack(CenterStackPlace place)
        {
            return this.CenterStacks[place.AsInt];
        }
        #endregion

        #region プロパティ（プレイヤー別）
        /// <summary>
        /// ゲーム・モデル・バッファー
        /// 
        /// - プレイヤー別
        /// </summary>
        Player[] Players { get; set; }

        internal Player GetPlayer(ModelOfThinkingEngine.Player player)
        {
            return this.Players[player.AsInt];
        }
        #endregion
    }
}
