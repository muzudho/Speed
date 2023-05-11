namespace Assets.Scripts.ThinkingEngine.Models.Game.Writer
{
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;

    /// <summary>
    /// ゲーム・モデル
    /// 
    /// - 書き込み専用
    /// </summary>
    class Model
    {
        // - その他

        public Model(ModelOfGameBuffer.Model gameModelBuffer)
        {
            this.gameModelBuffer = gameModelBuffer;
        }

        // - フィールド

        ModelOfGameBuffer.Model gameModelBuffer;
    }
}
