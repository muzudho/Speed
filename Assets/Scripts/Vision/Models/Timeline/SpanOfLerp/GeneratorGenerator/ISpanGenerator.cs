namespace Assets.Scripts.Vision.Models.Timeline.SpanOfLerp.GeneratorGenerator
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using SimulatorsOfTimeline = Assets.Scripts.Vision.Models.Timeline.SpanOfLerp.TimedGenerator;
    using VisionOfTimelineO4thElement = Assets.Scripts.Vision.Models.Timeline.O4thElement;

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
            LazyArgs.SetValue<VisionOfTimelineO4thElement.Model> setSpanToLerp);
    }
}
