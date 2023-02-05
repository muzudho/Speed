namespace Assets.Scripts.Gui.SpanOfLerp.Generator.Elements
{
    using Assets.Scripts.ThikningEngine;
    using Assets.Scripts.Views.Timeline;
    using SimulatorsOfTimeline = Assets.Scripts.Simulators;

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
            LazyArgs.SetValue<SpanToLerp> setSpanToLerp);
    }
}
