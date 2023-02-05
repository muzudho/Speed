namespace Assets.Scripts.Gui.SpanOfLerp.GeneratorGenerator
{
    using Assets.Scripts.ThinkingEngine;
    using SimulatorsOfTimeline = Assets.Scripts.Gui.SpanOfLerp.TimedGenerator;
    using SpanOfLeap = Assets.Scripts.Gui.SpanOfLerp;

    /// <summary>
    /// スパン生成器
    /// 
    /// - タイムライン上に配置されたもの
    /// </summary>
    interface ISpanGenerator
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        ISpanGenerator Spawn();

        // - プロパティ

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        /// <param name="gameModelBuffer">ゲームの内部状態（編集可能）</param>
        void CreateSpanToLerp(
            SimulatorsOfTimeline.TimedGenerator timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<SpanOfLeap.Model> setSpanToLerp);
    }
}
