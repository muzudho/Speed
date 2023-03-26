namespace Assets.Scripts.ThinkingEngine.Models.Game
{
    using System.Collections.Generic;
    using ModelOfThinkingEngine = Assets.Scripts.ThinkingEngine.Models;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.GameBuffer;

    /// <summary>
    /// ゲーム・モデル
    /// 
    /// - 台札別
    /// </summary>
    internal class CenterStack
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="placeObj"></param>
        internal CenterStack(
            ModelOfGameBuffer.Model gameModelBuffer,
            CenterStackPlace placeObj)
        {
            this.gameModelBuffer = gameModelBuffer;
            this.placeObj = placeObj;
        }

        // - フィールド

        readonly ModelOfGameBuffer.Model gameModelBuffer;

        readonly CenterStackPlace placeObj;
    }
}
