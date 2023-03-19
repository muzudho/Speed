namespace Assets.Scripts.Vision.Models.Timeline.SpanOfLerp.GeneratorGenerator
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using SimulatorsOfTimeline = Assets.Scripts.Vision.Models.Timeline.SpanOfLerp.TimedGenerator;
    using VisionOfTimelineO4thElement = Assets.Scripts.Vision.Models.Timeline.O4thElement;

    /// <summary>
    /// なんにもしません
    /// </summary>
    class SetIdlingView : AbstractSpanGenerator
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override ISpanGenerator Spawn()
        {
            return new SetIdlingView();
        }

        // - フィールド

        // - プロパティ

        SetIdling GetModel(SimulatorsOfTimeline.TimedGenerator timedGenerator)
        {
            return (SetIdling)timedGenerator.TimedCommandArg.CommandArg;
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        public override void CreateSpanToLerp(
            SimulatorsOfTimeline.TimedGenerator timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<VisionOfTimelineO4thElement.Model> setViewMovement)
        {
            // なんにもしません
        }
    }
}
