namespace Assets.Scripts.Vision.World.SpanOfLerp.GeneratorGenerator
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Model;
    using Assets.Scripts.ThinkingEngine.Model.CommandArgs;
    using SimulatorsOfTimeline = Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator;
    using SpanOfLeap = Assets.Scripts.Vision.World.SpanOfLerp;

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
            LazyArgs.SetValue<SpanOfLeap.Model> setViewMovement)
        {
            // なんにもしません
        }
    }
}
