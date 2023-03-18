namespace Assets.Scripts.Vision.World.SpanOfLerp.GeneratorGenerator
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using SimulatorsOfTimeline = Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator;
    using SpanOfLeap = Assets.Scripts.Vision.World.SpanOfLerp;

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
